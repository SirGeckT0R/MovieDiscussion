﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.UserProfileSpecifications;
using MovieServiceDataAccess.Specifications.WatchlistSpecifications;
using MovieServiceDomain.Exceptions;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.Watchlists.Commands.CreateWatchlistCommand
{
    public class CreateWatchlistHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateWatchlistHandler> logger) : ICommandHandler<CreateWatchlistCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateWatchlistHandler> _logger = logger;

        public async Task<Unit> Handle(CreateWatchlistCommand request, CancellationToken cancellationToken)
        {
            var profileSpecification = new UserProfileByAccountIdSpecification(request.AccountId);
            var candidates = await _unitOfWork.UserProfiles.GetWithSpecificationAsync(profileSpecification, cancellationToken);
            var candidateProfile = candidates.SingleOrDefault();

            if (candidateProfile == null)
            {
                _logger.LogError("Create watchlist command failed: user profile with account id {Id} not found", request.AccountId);

                throw new NotFoundException("User profile not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var watchlistSpecification = new WatchlistByProfileIdSpecification(candidateProfile.Id);
            var candidateWatchlists = await _unitOfWork.Watchlists.GetWithSpecificationAsync(watchlistSpecification, cancellationToken);
            var candidate = candidateWatchlists.SingleOrDefault();

            if (candidate != null)
            {
                _logger.LogError("Create watchlist command failed: watchlist was already created");

                throw new ConflictException("Watchlist was already created");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var watchlist = _mapper.Map<Watchlist>(request);
            watchlist.ProfileId = candidateProfile.Id;
            await _unitOfWork.Watchlists.AddAsync(watchlist, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
