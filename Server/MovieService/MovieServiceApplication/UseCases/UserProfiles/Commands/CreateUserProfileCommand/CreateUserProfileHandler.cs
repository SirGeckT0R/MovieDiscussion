using AutoMapper;
using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.UserProfileSpecifications;
using MovieServiceDomain.Exceptions;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.UserProfiles.Commands.CreateUserProfileCommand
{
    public class CreateUserProfileHandler(IUnitOfWork unitOfWork, IMapper mapper) : ICommandHandler<CreateUserProfileCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<Unit> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var profileSpecification = new UserProfileByAccountIdSpecification(request.AccountId);
            var candidates = await _unitOfWork.UserProfiles.GetWithSpecificationAsync(profileSpecification, cancellationToken);
            var candidateProfile = candidates.SingleOrDefault();

            if (candidateProfile != null)
            {
                throw new ConflictException("User profile already exists");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var userProfile = _mapper.Map<UserProfile>(request);
            await _unitOfWork.UserProfiles.AddAsync(userProfile, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
