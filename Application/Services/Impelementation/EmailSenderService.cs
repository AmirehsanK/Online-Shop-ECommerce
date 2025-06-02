using Application.Services.Interfaces;
using Application.Tools;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Impelementation;

public class EmailSenderService(IConfiguration configuration): IEmailSender
{  
    private readonly string _domainLink = configuration["ApplicationSettings:DomainLink"]!;
    
    /// <summary>
    /// Sends an email notification using the SendEmail utility.
    /// </summary>
    /// <param name="recipient">The email address of the recipient.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="body">The HTML or plain text body of the email.</param>
    /// <returns>A task that represents the asynchronous send operation.</returns>
    public Task SendEmailAsync(string recipient, string subject, string body)
    {
        try
        {
            EmailSender.Send(recipient, subject, body);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            // Log the exception later
            return Task.FromException(ex);
        }
    }
}