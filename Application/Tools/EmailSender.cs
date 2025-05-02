using System.Net;
using System.Net.Mail;

namespace Application.Tools;

public static class EmailSender
{
    public static async Task<bool> SendEmail(string to, string subject, string body)
    {
        var email = "Your Email Address";
        var password = "Your Password";

        try
        {
            var mail = new MailMessage();
            var smtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress(email, "فروشگاه ما");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            smtpServer.Port = 587;
            smtpServer.EnableSsl = true;


            smtpServer.Credentials = new NetworkCredential(email, password);
            smtpServer.Send(mail);

            return true;
        }
        catch (Exception exception)
        {
            return false;
        }
    }
}