using Grpc.Core;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.UserProfileSpecifications;
using MovieServiceDataAccess.Specifications.WatchlistSpecifications;
using MovieServiceDomain.Models;
namespace MovieGrpcServer.Services
{
    public class MovieServiceImpl(ILogger<MovieServiceImpl> logger, IUnitOfWork unitOfWork) : MovieService.MovieServiceBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<MovieServiceImpl> _logger = logger;

        public override async Task<CreateUserProfileReply> CreateUserProfile(CreateUserProfileRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GRPC movie service request started: Create User Profile for {Id}", request.AccountId);
            var cancellationToken = context.CancellationToken;

            Guid accountGuid;
            var isAccountIdValid = Guid.TryParse(request.AccountId, out accountGuid);

            if (!isAccountIdValid)
            {
                _logger.LogError("GRPC movie service request failed: Create User Profile: Account id {Id} is not valid", request.AccountId);

                throw new RpcException(new Status(StatusCode.InvalidArgument, "Account id is not valid"));
            }

            var profileSpecification = new UserProfileByAccountIdSpecification(accountGuid);
            var candidates = await _unitOfWork.UserProfiles.GetWithSpecificationAsync(profileSpecification, cancellationToken);
            var candidateProfile = candidates.SingleOrDefault();

            if (candidateProfile != null)
            {
                _logger.LogError("GRPC movie service request failed: Create User Profile: user profile with account id {Id} already exists", request.AccountId);

                throw new RpcException(new Status(StatusCode.AlreadyExists, "User profile already exists"));
            }

            cancellationToken.ThrowIfCancellationRequested();
            var userProfile = new UserProfile(accountGuid, request.Username);
            var profileId = await _unitOfWork.UserProfiles.AddAsync(userProfile, cancellationToken);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            var watchlist = new Watchlist(profileId, new List<Guid>());
            await _unitOfWork.Watchlists.AddAsync(watchlist, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var reply = new CreateUserProfileReply { IsSuccessfull = true };

            _logger.LogInformation("GRPC movie service request compeleted successfully: Create User Profile for {Id}", request.AccountId);

            return reply;
        }

        public override async Task<DeleteUserProfileReply> DeleteUserProfile(DeleteUserProfileRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GRPC movie service request started: Delete User Profile for {Id}", request.AccountId);
            var cancellationToken = context.CancellationToken;

            Guid accountGuid;
            var isAccountIdValid = Guid.TryParse(request.AccountId, out accountGuid);

            if (!isAccountIdValid)
            {
                _logger.LogError("GRPC movie service request failed: Delete User Profile: Account id {Id} is not valid", request.AccountId);

                throw new RpcException(new Status(StatusCode.InvalidArgument, "Account id is not valid"));
            }

            var profileSpecification = new UserProfileByAccountIdSpecification(accountGuid);
            var candidates = await _unitOfWork.UserProfiles.GetWithSpecificationAsync(profileSpecification, cancellationToken);
            var candidateProfile = candidates.SingleOrDefault();

            if (candidateProfile == null)
            {
                _logger.LogError("GRPC movie service request failed: Delete User Profile: Account id {Id} is not valid", request.AccountId);

                throw new RpcException(new Status(StatusCode.NotFound, "User profile not found"));
            }

            cancellationToken.ThrowIfCancellationRequested();
            var watchlistSpecification = new WatchlistByProfileIdSpecification(candidateProfile.Id);
            var candidateWatchlists = await _unitOfWork.Watchlists.GetWithSpecificationAsync(watchlistSpecification, cancellationToken);
            var watchlist = candidateWatchlists.SingleOrDefault();

            if (watchlist == null)
            {
                _logger.LogError("GRPC movie service request failed: Delete User Profile: Watchlist not found");

                throw new RpcException(new Status(StatusCode.NotFound, "Watchlist not found"));
            }

            cancellationToken.ThrowIfCancellationRequested();
            _unitOfWork.Watchlists.Delete(watchlist, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            _unitOfWork.UserProfiles.Delete(candidateProfile, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var reply = new DeleteUserProfileReply { IsSuccessfull = true };

            _logger.LogInformation("GRPC movie service request compeleted successfully: Delete User Profile for {Id}", request.AccountId);

            return reply;
        }
    }
}
