using Microsoft.Extensions.Options;
using school_major_project.Configuration;
using school_major_project.Interfaces;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace school_major_project.ModelServices
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_settings.FromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };
            message.To.Add(to);

            using (var client = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort))
            {
                client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
                client.EnableSsl = true;

                await client.SendMailAsync(message);
            }
        }

        public async Task SendEmailAsync(string to, string subject, string body, byte[] attachment, string attachmentName = "qrcode.png", bool isHtml = false)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_settings.FromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };
            message.To.Add(to);

            if (attachment != null)
            {
                message.Attachments.Add(new Attachment(new MemoryStream(attachment), attachmentName, "image/png"));
            }

            using (var client = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort))
            {
                client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
                client.EnableSsl = true;

                await client.SendMailAsync(message);
            }
        }
    }
}
