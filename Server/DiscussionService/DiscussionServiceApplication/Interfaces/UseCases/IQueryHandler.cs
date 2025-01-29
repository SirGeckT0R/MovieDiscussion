using MediatR;

namespace DiscussionServiceApplication.Interfaces.UseCases
{
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
    }
}
