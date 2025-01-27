using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.UserProfileSpecifications;
using MovieServiceDataAccess.Specifications.WatchlistSpecifications;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Watchlists.Queries.GetWatchlistByAccountIdQuery
{
    public class GetWatchlistByAccountIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetWatchlistByAccountIdQuery, WatchlistDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<WatchlistDto> Handle(GetWatchlistByAccountIdQuery request, CancellationToken cancellationToken)
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

            return _mapper.Map<WatchlistDto>(watchlist);
        }
    }
}
