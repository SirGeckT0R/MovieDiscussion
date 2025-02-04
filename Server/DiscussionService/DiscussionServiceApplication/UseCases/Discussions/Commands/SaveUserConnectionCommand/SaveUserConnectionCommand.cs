using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.SaveUserConnectionCommand
{
    public record SaveUserConnectionCommand(string DiscussionId, string AccountIdClaimValue, string ConnectionId) : ICommand<UserConnection>;
}
