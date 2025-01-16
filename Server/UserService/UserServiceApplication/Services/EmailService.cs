using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using UserServiceApplication.Interfaces.Services;

namespace UserServiceApplication.Services
{
    public class EmailService : IEmailService
    {
        private readonly string? email;
        private readonly string? password;
        public EmailService(IConfiguration configuration)
        {
            email = configuration["SMTPServerEmail"];
            password = configuration["SMTPServerPassword"];
        }
        public Task SendEmailAsync(string recipientEmail, string subject, string message, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
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

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(email);
            mailMessage.To.Add(new MailAddress(recipientEmail));
            mailMessage.Subject = subject;
            mailMessage.Body = message;


            cancellationToken.ThrowIfCancellationRequested();
            return client.SendMailAsync(mailMessage);
        }
    }
}
