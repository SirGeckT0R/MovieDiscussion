using AutoMapper;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.UserProfileSpecifications;
using MovieServiceDataAccess.Specifications.WatchlistSpecifications;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Watchlists.Queries.GetWatchlistByAccountIdQuery
{
    public class GetWatchlistByAccountIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetWatchlistByAccountIdHandler> logger) : IQueryHandler<GetWatchlistByAccountIdQuery, WatchlistDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetWatchlistByAccountIdHandler> _logger = logger;

        public async Task<WatchlistDto> Handle(GetWatchlistByAccountIdQuery request, CancellationToken cancellationToken)
        {
            var profileSpecification = new UserProfileByAccountIdSpecification(request.AccountId);
            var candidates = await _unitOfWork.UserProfiles.GetWithSpecificationAsync(profileSpecification, cancellationToken);
            var candidateProfile = candidates.SingleOrDefault();

            if (candidateProfile == null)
            {
                _logger.LogError("Get watchlist by account id query failed: user profile with account id {Id} not found", request.AccountId);

                throw new NotFoundException("User profile not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var watchlistSpecification = new WatchlistByProfileIdSpecification(candidateProfile.Id);
            var candidateWatchlists = await _unitOfWork.Watchlists.GetWithSpecificationAsync(watchlistSpecification, cancellationToken);
            var watchlist = candidateWatchlists.SingleOrDefault();

            if (watchlist == null)
            {
                _logger.LogError("Get watchlist by account id {Id} query failed: watchlist not found", request.AccountId);

                throw new NotFoundException("Watchlist not found");
            }

            var watchlistDto =_mapper.Map<WatchlistDto>(watchlist);

            cancellationToken.ThrowIfCancellationRequested();
            var movies = await _unitOfWork.Movies.GetFromListOfIdsAsync(watchlist.MovieIds, cancellationToken);
            var moviesDto = _mapper.Map<ICollection<MovieDto>>(movies);
            watchlistDto.Movies = moviesDto;

            return watchlistDto;
        }
    }
}
