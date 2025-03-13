using DiscussionServiceApplication.Interfaces.UseCases;
using MediatR;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.CreateDiscussionCommand
{
    public record CreateDiscussionCommand(Guid? CreatedBy, string Title, string Description, DateTime StartAt) : ICommand<Unit>;
}
