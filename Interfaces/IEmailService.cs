namespace school_major_project.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
        Task SendEmailAsync(string to, string subject, string body, byte[] attachment, string attachmentName = "qrcode.png", bool isHtml = false);
    }
}
