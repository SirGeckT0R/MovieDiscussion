using AutoMapper;
using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceApplication.Jobs;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Models;
using Hangfire;
using MediatR;
using UserGrpcClient;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.CreateDiscussionCommand
{
    public class CreateDiscussionHandler(IUnitOfWork unitOfWork, IMapper mapper, UserService.UserServiceClient client) : ICommandHandler<CreateDiscussionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly UserService.UserServiceClient _userServiceClient = client;

        public async Task<Unit> Handle(CreateDiscussionCommand request, CancellationToken cancellationToken)
        {
            var getUserInfoRequest = new GetUserInfoRequest { UserId = request.CreatedBy.ToString() };

            _ = await _userServiceClient.GetUserInfoAsync(getUserInfoRequest);

            var discussion = _mapper.Map<Discussion>(request);
            await _unitOfWork.Discussions.AddAsync(discussion, cancellationToken);

            BackgroundJob.Schedule<DiscussionActivationJob>(x => x.ExecuteAsync(discussion.Id), discussion.StartAt);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
