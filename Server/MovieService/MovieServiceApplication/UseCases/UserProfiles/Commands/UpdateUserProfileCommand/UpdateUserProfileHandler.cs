using AutoMapper;
using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.UserProfileSpecifications;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.UserProfiles.Commands.UpdateUserProfileCommand
{
    public class UpdateUserProfileHandler(IUnitOfWork unitOfWork, IMapper mapper) : ICommandHandler<UpdateUserProfileCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = (await _unitOfWork.UserProfiles.GetWithSpecificationAsync(new UserProfileByAccountIdSpecification(request.AccountId), cancellationToken)).SingleOrDefault()
                            ?? throw new NotFoundException("Profile not found");

            cancellationToken.ThrowIfCancellationRequested();
            var newProfile = _mapper.Map(request, profile);
            _unitOfWork.UserProfiles.Update(newProfile, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
