﻿using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Watchlists.Queries.GetWatchlistByIdQuery
{
    public class GetWatchlistByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetWatchlistByIdQuery, WatchlistDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<WatchlistDto> Handle(GetWatchlistByIdQuery request, CancellationToken cancellationToken)
        {
            var watchlist = await _unitOfWork.Watchlists.GetByIdAsync(request.Id, cancellationToken);

            if (watchlist == null)
            {
                throw new NotFoundException("Watchlist not found");
            }

            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<WatchlistDto>(watchlist);
        }
    }
}
