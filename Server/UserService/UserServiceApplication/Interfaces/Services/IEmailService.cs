namespace UserServiceApplication.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipientEmail, string subject, string message, CancellationToken cancellationToken);
    }
}
