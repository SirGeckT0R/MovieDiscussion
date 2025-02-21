namespace DiscussionServiceApplication.RabbitMQ.Options
{
    public class RabbitMQConnectionOptions
    {
        public string HostName { get; set; } = string.Empty;
        public int Port { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string SubscriptionQueueName { get; set; } = string.Empty;
    }
}
