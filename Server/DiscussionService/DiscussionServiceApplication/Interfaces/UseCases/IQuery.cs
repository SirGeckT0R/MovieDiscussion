using MediatR;
namespace DiscussionServiceApplication.Interfaces.UseCases
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}
