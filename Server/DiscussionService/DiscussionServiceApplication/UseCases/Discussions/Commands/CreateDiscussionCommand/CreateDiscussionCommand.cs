using DiscussionServiceApplication.Interfaces.UseCases;
using MediatR;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.CreateDiscussionCommand
{
    public record CreateDiscussionCommand(string Title, string Description, DateTime StartDateTime, Guid CreatorProfileId) : ICommand<Unit>;
}
