using System.Net.Mail;
using System.Net;
using Tedu.Identity.Infrastructure.Settings;

namespace Tedu.Identity.IDP.Services;

public sealed class SmtpMailService : IEmailSender
{
    private readonly SmtpEmailSettings settings;

    public SmtpMailService(SmtpEmailSettings settings)
    {
         this.settings = settings;
    }

    public void SendEmail(string recipient, string subject, string body, bool isBodyHtml = false, string? sender = null)
    {
        var message = new MailMessage(this.settings.From, recipient)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = isBodyHtml,
            From = new MailAddress(settings.From, !string.IsNullOrEmpty(sender) ? sender : settings.From),
        };

        using var client = new SmtpClient(settings.SmtpServer, settings.Port)
        {
            EnableSsl = settings.UseSsl
        };

        if (!string.IsNullOrWhiteSpace(settings.Username) || !string.IsNullOrWhiteSpace(settings.Password))
        {
            client.Credentials = new NetworkCredential(settings.Username, settings.Password);
        }
        else
        {
            client.UseDefaultCredentials = true;
        }

        client.Send(message);
    }
}
