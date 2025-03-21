using school_major_project.Interfaces;
using System.Net.Mail;
using System.Net;

namespace school_major_project.ModelServices
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _fromEmail;
        private readonly string _username;
        private readonly string _password;
        public EmailService(string smtpServer, int smtpPort, string fromEmail, string username, string password)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _fromEmail = fromEmail;
            _username = username;
            _password = password;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };
            message.To.Add(to);

            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Credentials = new NetworkCredential(_username, _password);
                client.EnableSsl = true;

                await client.SendMailAsync(message);
            }
        }
    }
}
