using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;

namespace DiscussionServiceApplication.UseCases.Discussions.Queries.GetDiscussionsByCreatorIdQuery
{
    public record GetDiscussionsByCreatorIdQuery(Guid CreatedBy) : IQuery<ICollection<DiscussionDto>>;
}
