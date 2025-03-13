using AutoMapper;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.ReviewSpecifications;
using MovieServiceDataAccess.Specifications.UserProfileSpecifications;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Reviews.Queries.GetReviewByMovieAndAccountIdQuery
{
    public class GetReviewByMovieAndAccountIdHandler(IUnitOfWork unitOfWork,
                                                     IMapper mapper,
                                                     ILogger<GetReviewByMovieAndAccountIdHandler> logger)
                                                     : IQueryHandler<GetReviewByMovieAndAccountIdQuery, ReviewDto?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetReviewByMovieAndAccountIdHandler> _logger = logger;

        public async Task<ReviewDto?> Handle(GetReviewByMovieAndAccountIdQuery request, CancellationToken cancellationToken)
        {
            var profileSpecification = new UserProfileByAccountIdSpecification(request.AccountId ?? Guid.Empty);
            var candidates = await _unitOfWork.UserProfiles.GetWithSpecificationAsync(profileSpecification, cancellationToken);
            var candidateProfile = candidates.SingleOrDefault();

            if (candidateProfile == null)
            {
                _logger.LogError("Get review command failed: user profile with account id {Id} not found", request.AccountId);

                throw new NotFoundException("User profile not found");
            }

            var reviewSpecification = new ReviewByProfileAndMovieIdSpecification(candidateProfile.Id, request.MovieId);
            var candidatesReviews = await _unitOfWork.Reviews.GetWithSpecificationAsync(reviewSpecification, cancellationToken);
            var candidateReview = candidatesReviews.SingleOrDefault();

            if (candidateReview == null)
            {
                _logger.LogWarning("Get review command failed: for movie with id {MovieId} and account with id {AccountId}", request.MovieId, request.AccountId);

                return null;
            }

            return _mapper.Map<ReviewDto>(candidateReview);
        }
    }
}
