using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;

namespace DiscussionServiceApplication.UseCases.Messages.Queries.GetMessageByIdQuery
{
    public record GetMessageByIdQuery(Guid Id) : IQuery<MessageDto>;
}
