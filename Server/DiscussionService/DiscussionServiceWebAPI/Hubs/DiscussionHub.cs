﻿using DiscussionServiceApplication.UseCases.UserConnections.Commands.RemoveUserConnectionCommand;
using DiscussionServiceApplication.UseCases.UserConnections.Commands.SaveUserConnectionCommand;
using DiscussionServiceApplication.UseCases.UserConnections.Queries.GetUserConnectionQuery;
using DiscussionServiceWebAPI.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace DiscussionServiceWebAPI.Hubs
{
    public class DiscussionHub(IMediator mediator) : Hub<IDiscussionHub>
    {
        private readonly IMediator _mediator = mediator;

        public async Task JoinChat(string DiscussionId)
        {
            var cancellationToken = Context.ConnectionAborted;

            var accountIdClaimValue = Context.User?.Claims.FirstOrDefault(x => x.Type == "AccountId")?.Value ?? string.Empty;

            var saveConnectionCommand = new SaveUserConnectionCommand(DiscussionId, accountIdClaimValue, Context.ConnectionId);
            var userConnection = await _mediator.Send(saveConnectionCommand, cancellationToken);

            var stringDiscussionId = userConnection.DiscussionId.ToString();

            await Groups.AddToGroupAsync(Context.ConnectionId, stringDiscussionId);

            await Clients
                    .Group(stringDiscussionId)
                    .ReceiveMessage("Admin", $"{userConnection.AccountId} joined the chat");
        }

        public async Task SendMessage(string message)
        {
            var cancellationToken = Context.ConnectionAborted;

            var getConnectionCommand = new GetUserConnectionQuery(Context.ConnectionId);
            var userConnection = await _mediator.Send(getConnectionCommand, cancellationToken);

            var stringDiscussionId = userConnection.DiscussionId.ToString();

            await Clients
                    .Group(stringDiscussionId)
                    .ReceiveMessage($"{userConnection.AccountId}", message);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            
            var removeConnectionCommand = new RemoveUserConnectionCommand(Context.ConnectionId);
            var userConnection = await _mediator.Send(removeConnectionCommand, cancellationToken);

            var stringDiscussionId = userConnection.DiscussionId.ToString();

            await Groups
                    .RemoveFromGroupAsync(Context.ConnectionId, stringDiscussionId);

            await Clients
                    .Group(stringDiscussionId)
                    .ReceiveMessage("Admin", $"{userConnection.AccountId} left the chat");

            await base.OnDisconnectedAsync(exception);
        }
    }
}
