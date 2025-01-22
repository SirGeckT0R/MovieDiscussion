using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.WatchlistSpecifications;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Watchlists.Queries.GetWatchlistByUserIdQuery
{
    public class GetWatchlistByUserIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetWatchlistByUserIdQuery, WatchlistDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<WatchlistDto> Handle(GetWatchlistByUserIdQuery request, CancellationToken cancellationToken)
        {
            var watchlist = await _unitOfWork.Watchlists.GetWithSpecificationAsync(new WatchlistByUserIdSpecification(request.UserId), cancellationToken) ?? throw new NotFoundException("Watchlist not found");

            return _mapper.Map<WatchlistDto>(watchlist);
        }
    }
}
