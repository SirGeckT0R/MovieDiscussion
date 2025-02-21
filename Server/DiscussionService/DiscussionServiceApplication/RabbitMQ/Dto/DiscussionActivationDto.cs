namespace DiscussionServiceApplication.RabbitMQ.Dto
{
    public record DiscussionActivationDto(string Title, ICollection<Guid> Subscribers);
}
