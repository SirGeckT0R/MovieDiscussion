using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;

namespace DiscussionServiceApplication.UseCases.Discussions.Queries.GetAllDiscussionsQuery
{
    public record GetAllDiscussionsQuery : IQuery<ICollection<DiscussionDto>>;
}
