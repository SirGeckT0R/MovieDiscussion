namespace DiscussionServiceWebAPI.Interfaces
{
    public interface IDiscussionHub 
    {
        Task ReceiveMessage(string username, string message);
    }
}
