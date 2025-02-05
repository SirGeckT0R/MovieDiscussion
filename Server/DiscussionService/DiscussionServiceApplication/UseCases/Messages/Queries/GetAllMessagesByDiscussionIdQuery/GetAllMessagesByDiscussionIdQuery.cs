using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;

namespace DiscussionServiceApplication.UseCases.Messages.Queries.GetAllMessagesByDiscussionIdQuery
{
    public record GetAllMessagesByDiscussionIdQuery(Guid DiscussionId) : IQuery<ICollection<MessageDto>>;
}
