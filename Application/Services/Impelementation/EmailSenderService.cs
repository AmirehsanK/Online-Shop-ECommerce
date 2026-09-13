using System.Net;
using System.Net.Mail;
using Application.Services.Interfaces;
using Application.Tools;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Services.Impelementation;

/// <summary>
/// Sends mail through the SMTP server in the "Smtp" configuration section. The address and
/// password used to be hard-coded in source - and a real Gmail app password sat in the
/// public history. Now they only ever come from configuration.
/// </summary>
public class EmailSenderService(IOptions<SmtpOptions> options) : IEmailSender
{
    private readonly SmtpOptions _options = options.Value;

    public async Task SendEmailAsync(string recipient, string subject, string body)
    {
        using var mail = new MailMessage
        {
            From = new MailAddress(_options.FromAddress ?? _options.UserName!, _options.FromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mail.To.Add(recipient);

        using var client = new SmtpClient(_options.Host, _options.Port)
        {
            EnableSsl = _options.EnableSsl,
            Credentials = new NetworkCredential(_options.UserName, _options.Password)
        };
        await client.SendMailAsync(mail);
    }
}

/// <summary>
/// Used when no SMTP host is configured: writes the email, links included, to the log so
/// sign-up activation and password reset can be completed locally and in the Docker demo.
/// </summary>
public class LoggingEmailSender(ILogger<LoggingEmailSender> logger) : IEmailSender
{
    public Task SendEmailAsync(string recipient, string subject, string body)
    {
        logger.LogWarning("SMTP is not configured; email not sent. To: {Recipient} Subject: {Subject} Body: {Body}",
            recipient, subject, body);
        return Task.CompletedTask;
    }
}
