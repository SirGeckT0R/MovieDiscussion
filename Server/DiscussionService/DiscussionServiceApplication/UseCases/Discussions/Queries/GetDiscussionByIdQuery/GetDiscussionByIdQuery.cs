using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;
namespace DiscussionServiceApplication.UseCases.Discussions.Queries.GetDiscussionByIdQuery
{
    public record GetDiscussionByIdQuery(Guid Id) : IQuery<DiscussionDto>;
}
