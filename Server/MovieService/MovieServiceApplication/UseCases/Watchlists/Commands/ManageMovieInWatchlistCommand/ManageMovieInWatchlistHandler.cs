using MediatR;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Enums;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.UserProfileSpecifications;
using MovieServiceDataAccess.Specifications.WatchlistSpecifications;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Watchlists.Commands.ManageMovieInWatchlistCommand
{
    public class ManageMovieInWatchlistHandler(IUnitOfWork unitOfWork, ILogger<ManageMovieInWatchlistHandler> logger) : ICommandHandler<ManageMovieInWatchlistCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<ManageMovieInWatchlistHandler> _logger = logger;

        public async Task<Unit> Handle(ManageMovieInWatchlistCommand request, CancellationToken cancellationToken)
        {
            var profileSpecification = new UserProfileByAccountIdSpecification(request.AccountId);
            var candidates = await _unitOfWork.UserProfiles.GetWithSpecificationAsync(profileSpecification, cancellationToken);
            var candidateProfile = candidates.SingleOrDefault();

            if (candidateProfile == null)
            {
                _logger.LogError("Manage movie in watchlist command failed: user profile not found");

                throw new NotFoundException("User profile not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var watchlistSpecification = new WatchlistByProfileIdSpecification(candidateProfile.Id);
            var candidateWatchlists = await _unitOfWork.Watchlists.GetWithSpecificationAsync(watchlistSpecification, cancellationToken);
            var watchlist = candidateWatchlists.SingleOrDefault();

            if (watchlist == null)
            {
                _logger.LogError("Manage movie in watchlist command failed: watchlist not found");

                throw new NotFoundException("Watchlist not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var movie = await _unitOfWork.Movies.GetByIdAsync(request.MovieId, cancellationToken);

            if (movie == null)
            {
                _logger.LogError("Manage movie in watchlist command failed: movie not found");

                throw new NotFoundException("Movie not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            switch (request.Action)
            {
                case WatchlistAction.Add:

                    if (watchlist.MovieIds.Any(m => m.Equals(request.MovieId)))
                    {
                        _logger.LogError("Manage movie in watchlist command failed: movie already added to watchlist");

                        throw new ConflictException("Movie already added to watchlist");
                    }

                    watchlist.MovieIds.Add(request.MovieId);
                    break;

                case WatchlistAction.Remove:

                    if (!watchlist.MovieIds.Remove(request.MovieId))
                    {
                        _logger.LogError("Manage movie in watchlist command failed: movie wasn't present in watchlist");

                        throw new ConflictException("Movie wasn't present in watchlist");
                    }

                    break;
            }

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
