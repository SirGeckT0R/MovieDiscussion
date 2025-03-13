using DiscussionServiceApplication.Interfaces.UseCases;
using MediatR;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.UpdateDiscussionCommand
{
    public record UpdateDiscussionCommand(Guid? UpdatedBy, Guid Id, string Title, string Description) : ICommand<Unit>;
}
