using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;

namespace DiscussionServiceApplication.UseCases.Messages.Commands.AddMessageToDiscussionCommand
{
    public record AddMessageToDiscussionCommand(string ConnectionId, string Text) : ICommand<UserConnection>;
}
