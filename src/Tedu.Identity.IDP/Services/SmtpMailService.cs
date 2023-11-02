using System.Net.Mail;
using System.Net;
using Tedu.Identity.IDP.Common.Settings;

namespace Tedu.Identity.IDP.Services;

public class SmtpMailService : IEmailSender
{
    private readonly SmtpEmailSettings _settings;

    public SmtpMailService(SmtpEmailSettings settings)
    {
         _settings = settings;
    }

    public void SendEmail(string recipient, string subject, string body, bool isBodyHtml = false, string? sender = null)
    {
        var message = new MailMessage(_settings.From, recipient)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = isBodyHtml,
            From = new MailAddress(_settings.From, !string.IsNullOrEmpty(sender) ? sender : _settings.From),
        };

        using var client = new SmtpClient(_settings.SmtpServer, _settings.Port)
        {
            EnableSsl = _settings.UseSsl
        };

        if (!string.IsNullOrWhiteSpace(_settings.Username) || !string.IsNullOrWhiteSpace(_settings.Password))
        {
            client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
        }
        else
        {
            client.UseDefaultCredentials = true;
        }

        client.Send(message);
    }
}
