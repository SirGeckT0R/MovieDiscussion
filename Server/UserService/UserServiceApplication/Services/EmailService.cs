using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using UserServiceApplication.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace UserServiceApplication.Services
{
    public class EmailService(IConfiguration configuration, ILogger<EmailService> logger) : IEmailService
    {
        private readonly ILogger<EmailService> _logger = logger;
        private readonly string? email = configuration["SMTPServerEmail"];
        private readonly string? password = configuration["SMTPServerPassword"];

        public Task SendEmailAsync(string recipientEmail, string subject, string message, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                _logger.LogWarning("No email or password provided. Sending email is ignored.");

                Console.WriteLine("No email or password provided. Sending email is ignored.");

                return Task.CompletedTask;
            }

            cancellationToken.ThrowIfCancellationRequested();
            var client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(email, password)
            };

            MailMessage mailMessage = new()
            {
                From = new MailAddress(email)
            };

            mailMessage.To.Add(new MailAddress(recipientEmail));
            mailMessage.Subject = subject;
            mailMessage.Body = message;

            cancellationToken.ThrowIfCancellationRequested();

            return client.SendMailAsync(mailMessage, cancellationToken);
        }
    }
}
