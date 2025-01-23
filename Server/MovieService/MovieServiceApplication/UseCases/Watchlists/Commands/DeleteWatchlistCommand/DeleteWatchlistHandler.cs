using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.WatchlistSpecifications;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Watchlists.Commands.DeleteWatchlistCommand
{
    public class DeleteWatchlistHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteWatchlistCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(DeleteWatchlistCommand request, CancellationToken cancellationToken)
        {
            var watchlist = (await _unitOfWork.Watchlists.GetWithSpecificationAsync(new WatchlistByProfileIdSpecification(request.ProfileId), cancellationToken)).SingleOrDefault() 
                                ?? throw new NotFoundException("Watchlist not found");

            cancellationToken.ThrowIfCancellationRequested();
            _unitOfWork.Watchlists.Delete(watchlist, cancellationToken);

            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
