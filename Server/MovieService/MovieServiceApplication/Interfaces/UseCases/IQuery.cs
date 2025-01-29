using MediatR;
namespace MovieServiceApplication.Interfaces.UseCases
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}
