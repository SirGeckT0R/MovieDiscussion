using DiscussionServiceApplication.UseCases.Discussions.Commands.RemoveUserConnectionCommand;
using DiscussionServiceApplication.UseCases.Discussions.Commands.SaveUserConnectionCommand;
using DiscussionServiceApplication.UseCases.Messages.Commands.AddMessageToDiscussionCommand;
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

            _logger.LogInformation("Attempt to join chat for discussion with id {Id} completed successfuly for {ConnectionId}", DiscussionId, Context.ConnectionId);

            await Groups.AddToGroupAsync(Context.ConnectionId, stringDiscussionId);

            await Clients
                    .Group(stringDiscussionId)
                    .ReceiveMessage(string.Empty, "Admin", $"{userConnection.Username} joined the chat", DateTime.UtcNow.ToString());
        }

        public async Task SendMessage(string Message)
        {
            _logger.LogInformation("Attempt to send message to a discussion started for {ConnectionId}", Context.ConnectionId);

            var cancellationToken = Context.ConnectionAborted;

            var saveMessageCommand = new AddMessageToDiscussionCommand(Context.ConnectionId, Message);
            var (userConnection, savedMessage) = await _mediator.Send(saveMessageCommand, cancellationToken);

            var stringUserId = userConnection.AccountId.ToString();
            var stringDiscussionId = userConnection.DiscussionId.ToString();

            _logger.LogInformation("Attempt to send message to a discussion completed successfuly for {ConnectionId}", Context.ConnectionId);

            await Clients
                    .Group(stringDiscussionId)
                    .ReceiveMessage(stringUserId, userConnection.Username, savedMessage.Text, savedMessage.SentAt.ToString());
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("Attempt to disconnect from a discussion started for {ConnectionId}", Context.ConnectionId);

            using var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            
            var removeConnectionCommand = new RemoveUserConnectionCommand(Context.ConnectionId);
            var userConnection = await _mediator.Send(removeConnectionCommand, cancellationToken);

            var stringDiscussionId = userConnection.DiscussionId.ToString();

            _logger.LogInformation("Attempt to disconnect from a discussion completed successfuly for {ConnectionId}", Context.ConnectionId);

            await Groups
                    .RemoveFromGroupAsync(Context.ConnectionId, stringDiscussionId);

            await Clients
                    .Group(stringDiscussionId)
                    .ReceiveMessage(string.Empty, "Admin", $"{userConnection.Username} left the chat", DateTime.UtcNow.ToString());

            await base.OnDisconnectedAsync(exception);
        }
    }
}
