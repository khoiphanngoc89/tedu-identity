namespace Tedu.Identity.IDP.Services;

public interface IEmailSender
{
    void SendEmail(string recipient, string subject, string body, bool isBodyHtml = false, string? sender = null);
}
