using AutoMapper;
using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.ReviewSpecifications;
using MovieServiceDataAccess.Specifications.UserProfileSpecifications;
using MovieServiceDomain.Exceptions;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.Reviews.Commands.AddReviewCommand
{
    public class AddReviewHandler(IUnitOfWork unitOfWork, IMapper mapper) : ICommandHandler<AddReviewCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var profileSpecification = new UserProfileByAccountIdSpecification(request.AccountId);
            var candidates = await _unitOfWork.UserProfiles.GetWithSpecificationAsync(profileSpecification, cancellationToken);
            var candidateProfile = candidates.SingleOrDefault();

            if (candidateProfile == null) 
            {
                throw new NotFoundException("User profile not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var candidateMovie = await _unitOfWork.Movies.GetByIdAsync(request.MovieId, cancellationToken);

            if (candidateMovie == null) 
            {
                throw new NotFoundException("Movie not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var reviewSpecification = new ReviewByProfileAndMovieIdSpecification(candidateProfile.Id, request.MovieId);
            var candidatesReviews = await _unitOfWork.Reviews.GetWithSpecificationAsync(reviewSpecification, cancellationToken);
            var candidateReview = candidatesReviews.SingleOrDefault();

            if (candidateReview != null)
            {
                throw new ConflictException("Review by that user for the movie already exists");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var review = _mapper.Map<Review>(request);
            review.ProfileId = candidateProfile.Id;
            await _unitOfWork.Reviews.AddAsync(review, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
