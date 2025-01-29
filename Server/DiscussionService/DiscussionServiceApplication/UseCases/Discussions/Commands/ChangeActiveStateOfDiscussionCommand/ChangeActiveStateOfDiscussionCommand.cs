using DiscussionServiceApplication.Interfaces.UseCases;
using MediatR;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.ChangeActiveStateOfDiscussionCommand
{
    public record ChangeActiveStateOfDiscussionCommand(Guid Id, bool NewState) : ICommand<Unit>;
}
