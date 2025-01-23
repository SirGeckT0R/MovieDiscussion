using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.UserProfileSpecifications;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.UserProfiles.Commands.DeleteUserProfileCommand
{
    public class DeleteUserProfileHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteUserProfileCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Unit> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = (await _unitOfWork.UserProfiles.GetWithSpecificationAsync(new UserProfileByAccountIdSpecification(request.AccountId), cancellationToken)).SingleOrDefault()
                            ?? throw new NotFoundException("Profile not found");

            cancellationToken.ThrowIfCancellationRequested();
            _unitOfWork.UserProfiles.Delete(profile, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
