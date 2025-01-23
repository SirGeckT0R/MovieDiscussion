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
            var profile = (await _unitOfWork.UserProfiles.GetWithSpecificationAsync(new UserProfileByAccountIdSpecification(request.AccountId), cancellationToken)).SingleOrDefault() 
                            ?? 
                            throw new NotFoundException("User profile not found");
            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<UserProfileDto>(profile);
        }
    }
}
