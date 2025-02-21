using AutoMapper;
using Grpc.Core;
using UserServiceDataAccess.Interfaces;

namespace UserGrpcServer.Services
{
    public class UserServiceImpl(ILogger<UserServiceImpl> logger, 
                                 IMapper mapper,
                                 IUserUnitOfWork userUnitOfWork) : UserService.UserServiceBase
    {
        private readonly IUserUnitOfWork _userUnitOfWork = userUnitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UserServiceImpl> _logger = logger;

        public override async Task<GetUserInfoReply> GetUserInfo(GetUserInfoRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GRPC user service request started: Get Username for id {Id}", request.UserId);

            Guid userGuid;
            var isUserIdValid = Guid.TryParse(request.UserId, out userGuid);

            if (!isUserIdValid) 
            {
                _logger.LogError("GRPC user service request failed: Get Username for id {Id}: User id is not valid", request.UserId);

                throw new RpcException(new Status(StatusCode.InvalidArgument, "User id is not valid"));
            }

            var user = await _userUnitOfWork.UserRepository.GetByIdAsync(userGuid, context.CancellationToken);

            if (user == null)
            {
                _logger.LogError("GRPC user service request failed: Get Username for id {Id}: User not found", request.UserId);

                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }

            var reply = _mapper.Map<GetUserInfoReply>(user);

            _logger.LogInformation("GRPC user service request completed succesfully: GetUsername for id {Id}", request.UserId);

            return reply;
        }
    }
}
