﻿namespace DiscussionServiceApplication.RabbitMQ.Dto
{
    public record DiscussionActivationDto(string DiscussionName, ICollection<Guid> Subscribers);
}
