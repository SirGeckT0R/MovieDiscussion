using MediatR;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Movies.Commands.ManageNotApprovedMovieCommand
{
    public class ManageNotApprovedMovieHandler(IUnitOfWork unitOfWork,
                                               ILogger<ManageNotApprovedMovieCommand> logger)
                                               : ICommandHandler<ManageNotApprovedMovieCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<ManageNotApprovedMovieCommand> _logger = logger;

        public async Task<Unit> Handle(ManageNotApprovedMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = await _unitOfWork.Movies.GetNotApprovedMovieByIdAsync(request.MovieId, cancellationToken);

            if (movie == null)
            {
                _logger.LogError("Manage not approved movie command attempt failed: no movie found for {Id}", request.MovieId);

                throw new NotFoundException("No movie found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            if (request.ShouldApprove)
            {
                movie.IsApproved = true;
                _unitOfWork.Movies.Update(movie, cancellationToken);
            }
            else
            {
                _unitOfWork.Movies.Delete(movie, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
