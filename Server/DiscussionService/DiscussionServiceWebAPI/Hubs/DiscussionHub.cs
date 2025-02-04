using DiscussionServiceApplication.UseCases.Discussions.Commands.AddMessageToDiscussionCommand;
using DiscussionServiceApplication.UseCases.Discussions.Commands.RemoveUserConnectionCommand;
using DiscussionServiceApplication.UseCases.Discussions.Commands.SaveUserConnectionCommand;
using DiscussionServiceWebAPI.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace DiscussionServiceWebAPI.Hubs
{
    public class DiscussionHub(IMediator mediator, ILogger<DiscussionHub> logger) : Hub<IDiscussionHub>
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<DiscussionHub> _logger = logger;

        public async Task JoinChat(string DiscussionId)
        {
            _logger.LogInformation("Attempt to join chat for discussion with id {Id} for {ConnectionId}", DiscussionId, Context.ConnectionId);

            var cancellationToken = Context.ConnectionAborted;

            var accountIdClaimValue = Context.User?.Claims.FirstOrDefault(x => x.Type == "AccountId")?.Value ?? string.Empty;

            var saveConnectionCommand = new SaveUserConnectionCommand(DiscussionId, accountIdClaimValue, Context.ConnectionId);
            var userConnection = await _mediator.Send(saveConnectionCommand, cancellationToken);

            var stringDiscussionId = userConnection.DiscussionId.ToString();

            _logger.LogInformation("Attempt to join chat for discussion with id {Id} was successful for {ConectionId}", DiscussionId, Context.ConnectionId);

            await Groups.AddToGroupAsync(Context.ConnectionId, stringDiscussionId);

            await Clients
                    .Group(stringDiscussionId)
                    .ReceiveMessage("Admin", $"{userConnection.AccountId} joined the chat");
        }

        public async Task SendMessage(string Message)
        {
            _logger.LogInformation("Attempt to send message to a discussion started for {ConectionId}", Context.ConnectionId);

            var cancellationToken = Context.ConnectionAborted;

            var saveMessageCommand = new AddMessageToDiscussionCommand(Context.ConnectionId, Message);
            var userConnection = await _mediator.Send(saveMessageCommand, cancellationToken);

            var stringDiscussionId = userConnection.DiscussionId.ToString();

            _logger.LogInformation("Attempt to send message to a discussion was successful for {ConectionId}", Context.ConnectionId);

            await Clients
                    .Group(stringDiscussionId)
                    .ReceiveMessage($"{userConnection.AccountId}", Message);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("Attempt to disconnect from a discussion started for {ConectionId}", Context.ConnectionId);

            using var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            
            var removeConnectionCommand = new RemoveUserConnectionCommand(Context.ConnectionId);
            var userConnection = await _mediator.Send(removeConnectionCommand, cancellationToken);

            var stringDiscussionId = userConnection.DiscussionId.ToString();

            _logger.LogInformation("Attempt to disconnect from a discussion was successful for {ConectionId}", Context.ConnectionId);

            await Groups
                    .RemoveFromGroupAsync(Context.ConnectionId, stringDiscussionId);

            await Clients
                    .Group(stringDiscussionId)
                    .ReceiveMessage("Admin", $"{userConnection.AccountId} left the chat");

            await base.OnDisconnectedAsync(exception);
        }
    }
}
