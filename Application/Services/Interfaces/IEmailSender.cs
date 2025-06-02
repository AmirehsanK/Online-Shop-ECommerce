namespace Application.Services.Interfaces;

public interface IEmailSender
{
    /// <summary>
    /// Sends an Email asynchronously.
    /// </summary>
    /// <param name="recipient">The recipient of the Email.</param>
    /// <param name="subject">The subject of the Email.</param>
    /// <param name="body">The main content/body of the Email.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendEmailAsync(string recipient, string subject, string body);
}