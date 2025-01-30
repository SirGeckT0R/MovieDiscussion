using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;

namespace DiscussionServiceApplication.UseCases.Discussions.Queries.GetUserConnectionQuery
{
    public record GetUserConnectionQuery(string ConnectionId) : IQuery<UserConnection>;
}
