using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;
using MediatR;

namespace DiscussionServiceApplication.UseCases.UserConnections.Commands.RemoveUserConnectionCommand
{
    public record RemoveUserConnectionCommand(string ConnectionId) : ICommand<UserConnection>;
}
