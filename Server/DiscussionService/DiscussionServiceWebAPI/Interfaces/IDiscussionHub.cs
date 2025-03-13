namespace DiscussionServiceWebAPI.Interfaces
{
    public interface IDiscussionHub 
    {
        Task ReceiveMessage(string userId, string username, string text, string sentAt);
    }
}
