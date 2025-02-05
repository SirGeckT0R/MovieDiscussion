using AutoMapper;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.UserProfileSpecifications;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.UserProfiles.Queries.GetProfileByAccountIdQuery
{
    public class GetProfileByAccountIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetProfileByAccountIdHandler> logger) : IQueryHandler<GetProfileByAccountIdQuery, UserProfileDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetProfileByAccountIdHandler> _logger = logger;

        public async Task<UserProfileDto> Handle(GetProfileByAccountIdQuery request, CancellationToken cancellationToken)
        {
            var profileSpecification = new UserProfileByAccountIdSpecification(request.AccountId);
            var candidates = await _unitOfWork.UserProfiles.GetWithSpecificationAsync(profileSpecification, cancellationToken);
            var candidateProfile = candidates.SingleOrDefault();

            if (candidateProfile == null)
            {
                _logger.LogError("Get user profile by account id query failed: user profile with account Id {Id} not found", request.AccountId);

                throw new NotFoundException("User profile not found");
            }

            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<UserProfileDto>(candidateProfile);
        }
    }
}
