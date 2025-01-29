using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.UserProfileSpecifications;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.UserProfiles.Queries.GetProfileByAccountIdQuery
{
    public class GetProfileByAccountIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetProfileByAccountIdQuery, UserProfileDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<UserProfileDto> Handle(GetProfileByAccountIdQuery request, CancellationToken cancellationToken)
        {
            var profileSpecification = new UserProfileByAccountIdSpecification(request.AccountId);
            var candidates = await _unitOfWork.UserProfiles.GetWithSpecificationAsync(profileSpecification, cancellationToken);
            var candidateProfile = candidates.SingleOrDefault();

            if (candidateProfile == null)
            {
                throw new NotFoundException("User profile not found");
            }

            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<UserProfileDto>(candidateProfile);
        }
    }
}
