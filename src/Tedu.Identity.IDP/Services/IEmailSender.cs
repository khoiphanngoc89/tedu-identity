namespace Tedu.Identity.IDP.Services;

public interface IEmailSender
{
    void SendEmail(string recipient, string subject, string body, bool isBodayHtml = false, string? sender = null);
}
