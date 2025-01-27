using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.ReviewSpecifications;
using MovieServiceDataAccess.Specifications.UserProfileSpecifications;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Reviews.Queries.GetReviewsByAccountIdQuery
{
    public class GetReviewsByAccountIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetReviewsByAccountIdQuery, ICollection<ReviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ICollection<ReviewDto>> Handle(GetReviewsByAccountIdQuery request, CancellationToken cancellationToken)
        {
            var profileSpecification = new UserProfileByAccountIdSpecification(request.AccountId);
            var candidates = await _unitOfWork.UserProfiles.GetWithSpecificationAsync(profileSpecification, cancellationToken);
            var candidateProfile = candidates.SingleOrDefault();

            if (candidateProfile == null)
            {
                throw new NotFoundException("User profile not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var reviewSpecification = new ReviewsByProfileIdSpecification(candidateProfile.Id);
            var reviews = await _unitOfWork.Reviews.GetWithSpecificationAsync(reviewSpecification, cancellationToken);

            return _mapper.Map<ICollection<ReviewDto>>(reviews);
        }
    }
}
