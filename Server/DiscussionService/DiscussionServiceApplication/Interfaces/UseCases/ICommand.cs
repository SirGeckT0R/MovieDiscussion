using MediatR;

namespace DiscussionServiceApplication.Interfaces.UseCases
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
