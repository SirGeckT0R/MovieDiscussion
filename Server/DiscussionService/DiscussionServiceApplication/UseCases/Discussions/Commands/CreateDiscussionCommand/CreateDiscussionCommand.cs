using DiscussionServiceApplication.Interfaces.UseCases;
using MediatR;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.CreateDiscussionCommand
{
    public record CreateDiscussionCommand(string Title, string Description, DateTime StartAt, Guid CreatedBy) : ICommand<Unit>;
}
