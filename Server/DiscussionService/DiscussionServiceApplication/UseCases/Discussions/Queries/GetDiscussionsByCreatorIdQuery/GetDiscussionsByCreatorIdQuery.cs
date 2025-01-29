using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;

namespace DiscussionServiceApplication.UseCases.Discussions.Queries.GetDiscussionsByCreatorIdQuery
{
    public record GetDiscussionsByCreatorIdQuery(Guid CreatorProfileId) : IQuery<ICollection<DiscussionDto>>;
}
