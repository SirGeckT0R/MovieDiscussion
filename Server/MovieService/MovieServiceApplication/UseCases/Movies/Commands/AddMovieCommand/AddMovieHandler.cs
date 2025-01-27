using AutoMapper;
using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.UserProfileSpecifications;
using MovieServiceDomain.Exceptions;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.Movies.Commands.AddMovieCommand
{
    public class AddMovieHandler(IUnitOfWork unitOfWork, IMapper mapper) : ICommandHandler<AddMovieCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(AddMovieCommand request, CancellationToken cancellationToken)
        {
            var profileSpecification = new UserProfileByAccountIdSpecification(request.AccountId);
            var candidates = await _unitOfWork.UserProfiles.GetWithSpecificationAsync(profileSpecification, cancellationToken);
            var candidateProfile = candidates.SingleOrDefault();

            if (candidateProfile == null)
            { 
                throw new NotFoundException("User profile not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var doGenresExist = _unitOfWork.Genres.DoExist(request.Genres, cancellationToken);

            if (!doGenresExist)
            {
                throw new NotFoundException("Some genres are not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var doCrewMembersExist = _unitOfWork.People.DoExist(request.CrewMembers.Select(x => x.PersonId).ToList(), cancellationToken);

            if (!doCrewMembersExist)
            {
                throw new NotFoundException("Some crew members are not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var movie = _mapper.Map<Movie>(request);
            movie.SubmittedBy = candidateProfile.Id;
            await _unitOfWork.Movies.AddAsync(movie, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
