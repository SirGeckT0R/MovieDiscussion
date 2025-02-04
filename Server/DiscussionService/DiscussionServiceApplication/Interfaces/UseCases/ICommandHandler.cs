using MediatR;

namespace DiscussionServiceApplication.Interfaces.UseCases
{
    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
    }
}
