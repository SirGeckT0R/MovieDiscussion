using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Movies.Commands.DeleteMovieCommand
{
    public class DeleteMovieHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteMovieCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = await _unitOfWork.Movies.GetByIdTrackingAsync(request.Id, cancellationToken);

            if (movie == null)
            {
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
