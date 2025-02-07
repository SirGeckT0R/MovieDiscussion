using MediatR;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Genres.Commands.DeleteGenreCommand
{
    public class DeleteGenreHandler(IUnitOfWork unitOfWork, ILogger<DeleteGenreHandler> logger) : ICommandHandler<DeleteGenreCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DeleteGenreHandler> _logger = logger;
        public async Task<Unit> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = await _unitOfWork.Genres.GetByIdTrackingAsync(request.Id, cancellationToken);

            if (genre == null)
            {
                _logger.LogError("Delete genre command failed for {Id}: genre not found", request.Id);

                throw new NotFoundException("Genre not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            _unitOfWork.Genres.Delete(genre, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
