using AutoMapper;
using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Movies.Commands.UpdateMovieCommand
{
    public class UpdateMovieHandler(IUnitOfWork unitOfWork, IMapper mapper) : ICommandHandler<UpdateMovieCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.Movies.GetByIdTrackingAsync(request.Id, cancellationToken);

            if (candidate == null)
            {
                throw new NotFoundException("Movie not found");
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
            var movie = _mapper.Map(request, candidate);
            _unitOfWork.Movies.Update(movie, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
