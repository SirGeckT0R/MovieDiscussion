using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.UserProfileSpecifications;
using MovieServiceDataAccess.Specifications.WatchlistSpecifications;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Watchlists.Commands.DeleteWatchlistCommand
{
    public class DeleteWatchlistHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteWatchlistCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(DeleteWatchlistCommand request, CancellationToken cancellationToken)
        {
            var profileSpecification = new UserProfileByAccountIdSpecification(request.AccountId);
            var candidates = await _unitOfWork.UserProfiles.GetWithSpecificationAsync(profileSpecification, cancellationToken);
            var candidateProfile = candidates.SingleOrDefault();
            if (candidateProfile == null)
            {
                throw new NotFoundException("User profile not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var watchlistSpecification = new WatchlistByProfileIdSpecification(candidateProfile.Id);
            var candidateWatchlists = await _unitOfWork.Watchlists.GetWithSpecificationAsync(watchlistSpecification, cancellationToken);
            var watchlist = candidateWatchlists.SingleOrDefault();
            if (watchlist == null)
            {
                throw new NotFoundException("Watchlist not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            _unitOfWork.Watchlists.Delete(watchlist, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
