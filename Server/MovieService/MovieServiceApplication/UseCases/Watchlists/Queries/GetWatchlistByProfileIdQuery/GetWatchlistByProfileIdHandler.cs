using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.WatchlistSpecifications;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Watchlists.Queries.GetWatchlistByProfileIdQuery
{
    public class GetWatchlistByProfileIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetWatchlistByProfileIdQuery, WatchlistDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<WatchlistDto> Handle(GetWatchlistByProfileIdQuery request, CancellationToken cancellationToken)
        {
            var watchlist = (await _unitOfWork.Watchlists.GetWithSpecificationAsync(new WatchlistByProfileIdSpecification(request.ProfileId), cancellationToken)).SingleOrDefault() 
                            ?? throw new NotFoundException("Watchlist not found");

            return _mapper.Map<WatchlistDto>(watchlist);
        }
    }
}
