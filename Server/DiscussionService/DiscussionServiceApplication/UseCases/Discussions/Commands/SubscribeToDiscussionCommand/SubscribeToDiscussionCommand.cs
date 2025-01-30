using DiscussionServiceApplication.Interfaces.UseCases;
using MediatR;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.SubscribeToDiscussionCommand
{
    public record SubscribeToDiscussionCommand(Guid DiscussionId, Guid AccountId) : ICommand<Unit>;
}
