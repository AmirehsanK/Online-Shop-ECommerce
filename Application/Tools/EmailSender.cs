using System.Net;
using System.Net.Mail;

namespace Application.Tools;

public static class EmailSender
{
    public static async Task<bool> SendEmail(string to, string subject, string body)
    {
        var email = "awref.ahngar@gmail.com";
        var password = "nfevaamqluuiainw";

        try
        {
            var mail = new MailMessage();
            var SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress(email, "فروشگاه ما");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpServer.Port = 587;
            SmtpServer.EnableSsl = true;


            SmtpServer.Credentials = new NetworkCredential(email, password);
            SmtpServer.Send(mail);

            return true;
        }
        catch (Exception exception)
        {
            return false;
        }
    }
}