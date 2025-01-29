using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;

namespace DiscussionServiceApplication.UseCases.UserConnections.Commands.SaveUserConnectionCommand
{
    public record SaveUserConnectionCommand(string DiscussionId, string AccountIdClaimValue, string ConnectionId) : ICommand<UserConnection>;
}
