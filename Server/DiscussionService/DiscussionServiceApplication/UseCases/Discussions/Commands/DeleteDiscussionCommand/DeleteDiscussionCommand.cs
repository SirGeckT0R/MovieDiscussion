using DiscussionServiceApplication.Interfaces.UseCases;
using MediatR;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.DeleteDiscussionCommand
{
    public record DeleteDiscussionCommand(Guid Id) : ICommand<Unit>;
}
