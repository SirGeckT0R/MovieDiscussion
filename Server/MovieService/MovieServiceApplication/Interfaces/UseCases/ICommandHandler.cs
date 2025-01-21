using MediatR;

namespace MovieServiceApplication.Interfaces.UseCases
{
    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
    }
}
