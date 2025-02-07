using AutoMapper;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Watchlists.Queries.GetWatchlistByIdQuery
{
    public class GetWatchlistByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetWatchlistByIdHandler> logger) : IQueryHandler<GetWatchlistByIdQuery, WatchlistDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetWatchlistByIdHandler> _logger = logger;

        public async Task<WatchlistDto> Handle(GetWatchlistByIdQuery request, CancellationToken cancellationToken)
        {
            var watchlist = await _unitOfWork.Watchlists.GetByIdAsync(request.Id, cancellationToken);

            if (watchlist == null)
            {
                _logger.LogError("Get watchlist by id query failed for {Id}: watchlist not found", request.Id);

                throw new NotFoundException("Watchlist not found");
            }

            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<WatchlistDto>(watchlist);
        }
    }
}
