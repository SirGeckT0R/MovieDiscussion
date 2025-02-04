using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Movies.Commands.UpdateMovieCommand
{
    public class UpdateMovieHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateMovieHandler> logger) : ICommandHandler<UpdateMovieCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdateMovieHandler> _logger = logger;

        public async Task<Unit> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.Movies.GetByIdTrackingAsync(request.Id, cancellationToken);

            if (candidate == null)
            {
                _logger.LogError("Update movie command failed for {Id}: movie not found", request.Id);

                throw new NotFoundException("Movie not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var doGenresExist = _unitOfWork.Genres.DoExist(request.Genres, cancellationToken);

            if (!doGenresExist)
            {
                _logger.LogError("Update movie command failed: some genres are not found");

                throw new NotFoundException("Some genres are not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var doCrewMembersExist = _unitOfWork.People.DoExist(request.CrewMembers.Select(x => x.PersonId).ToList(), cancellationToken);

            if (!doCrewMembersExist)
            {
                _logger.LogError("Update movie command failed: some crew members are not found");

                throw new NotFoundException("Some crew members are not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var movie = _mapper.Map(request, candidate);
            _unitOfWork.Movies.Update(movie, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
