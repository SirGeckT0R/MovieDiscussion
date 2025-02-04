using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.RemoveUserConnectionCommand
{
    public record RemoveUserConnectionCommand(string ConnectionId) : ICommand<UserConnection>;
}
