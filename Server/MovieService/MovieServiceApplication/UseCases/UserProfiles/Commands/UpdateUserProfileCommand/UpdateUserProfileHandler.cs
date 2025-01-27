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
            var profileSpecification = new UserProfileByAccountIdSpecification(request.AccountId);
            var candidates = await _unitOfWork.UserProfiles.GetWithSpecificationAsync(profileSpecification, cancellationToken);
            var candidateProfile = candidates.SingleOrDefault();

            if (candidateProfile == null)
            {
                throw new NotFoundException("User profile not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var newProfile = _mapper.Map(request, candidateProfile);
            _unitOfWork.UserProfiles.Update(newProfile, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
