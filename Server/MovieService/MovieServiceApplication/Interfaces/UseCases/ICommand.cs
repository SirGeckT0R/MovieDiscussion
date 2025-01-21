using MediatR;

namespace MovieServiceApplication.Interfaces.UseCases
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
