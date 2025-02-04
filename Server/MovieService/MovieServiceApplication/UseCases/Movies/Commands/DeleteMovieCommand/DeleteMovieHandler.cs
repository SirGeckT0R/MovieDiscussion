using MediatR;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Movies.Commands.DeleteMovieCommand
{
    public class DeleteMovieHandler(IUnitOfWork unitOfWork, ILogger<DeleteMovieHandler> logger) : ICommandHandler<DeleteMovieCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DeleteMovieHandler> _logger = logger;

        public async Task<Unit> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = await _unitOfWork.Movies.GetByIdTrackingAsync(request.Id, cancellationToken);

            if (movie == null)
            {
                _logger.LogError("Delete movie command failed: movie not found");

                throw new NotFoundException("Movie not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            _unitOfWork.Movies.Delete(movie, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
