using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Genres.Commands.DeleteGenreCommand
{
    public class DeleteGenreHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteGenreCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Unit> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = await _unitOfWork.Genres.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Genre not found");

            cancellationToken.ThrowIfCancellationRequested();
            _unitOfWork.Genres.Delete(genre, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
