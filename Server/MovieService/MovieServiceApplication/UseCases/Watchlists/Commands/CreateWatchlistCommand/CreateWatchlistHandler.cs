using AutoMapper;
using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.WatchlistSpecifications;
using MovieServiceDomain.Exceptions;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.Watchlists.Commands.CreateWatchlistCommand
{
    public class CreateWatchlistHandler(IUnitOfWork unitOfWork, IMapper mapper) : ICommandHandler<CreateWatchlistCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(CreateWatchlistCommand request, CancellationToken cancellationToken)
        {
            _ = await _unitOfWork.UserProfiles.GetByIdAsync(request.ProfileId, cancellationToken) ?? throw new NotFoundException("User profile not found");

            var candidate = (await _unitOfWork.Watchlists.GetWithSpecificationAsync(new WatchlistByProfileIdSpecification(request.ProfileId), cancellationToken)).SingleOrDefault();
            if (candidate != null)
            {
                throw new ConflictException("Watchlist was already created");
            }
        
            cancellationToken.ThrowIfCancellationRequested();
            var watchlist = _mapper.Map<Watchlist>(request);
            await _unitOfWork.Watchlists.AddAsync(watchlist, cancellationToken);

            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
