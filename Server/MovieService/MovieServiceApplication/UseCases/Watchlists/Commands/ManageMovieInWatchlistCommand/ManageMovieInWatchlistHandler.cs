using MediatR;
using MovieServiceApplication.Enums;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.UserProfileSpecifications;
using MovieServiceDataAccess.Specifications.WatchlistSpecifications;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Watchlists.Commands.ManageMovieInWatchlistCommand
{
    public class ManageMovieInWatchlistHandler(IUnitOfWork unitOfWork) : ICommandHandler<ManageMovieInWatchlistCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(ManageMovieInWatchlistCommand request, CancellationToken cancellationToken)
        {
            var candidateProfile = (await _unitOfWork.UserProfiles.GetWithSpecificationAsync(new UserProfileByAccountIdSpecification(request.AccountId), cancellationToken)).SingleOrDefault()
                                    ?? throw new NotFoundException("User profile not found");

            var watchlist = (await _unitOfWork.Watchlists.GetWithSpecificationAsync(new WatchlistByProfileIdSpecification(candidateProfile.Id), cancellationToken)).SingleOrDefault() 
                             ?? throw new NotFoundException("Watchlist not found");

            cancellationToken.ThrowIfCancellationRequested();
            _ = await _unitOfWork.Movies.GetByIdAsync(request.MovieId, cancellationToken) ?? throw new NotFoundException("Movie not found");

            cancellationToken.ThrowIfCancellationRequested();
            switch (request.Action)
            {
                case WatchlistAction.Add:
                    if (watchlist.MovieIds.Any(m => m.Equals(request.MovieId)))
                    {
                        throw new ConflictException("Movie already added to watchlist");
                    }
                    watchlist.MovieIds.Add(request.MovieId);
                    break;

                case WatchlistAction.Remove:
                    if (!watchlist.MovieIds.Remove(request.MovieId))
                    {
                        throw new ConflictException("Movie wasn't present in watchlist");
                    }
                    break;
             }

            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
