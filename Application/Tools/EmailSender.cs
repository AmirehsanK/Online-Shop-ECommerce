using System.Net;
using System.Net.Mail;

namespace Application.Tools;

public static class EmailSender
{
    public static void Send(string to, string subject, string body)
    {
        MailMessage mail = new MailMessage();
        SmtpClient smtpServer = new SmtpClient();
        mail.From = new MailAddress("your mail", "your name");
        mail.To.Add(to);
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        smtpServer.Host = "smtp.gmail.com";
        smtpServer.Port = 587;
        smtpServer.EnableSsl = true;
        smtpServer.Credentials = new System.Net.NetworkCredential("your mail", "password");

        smtpServer.Send(mail);

    }
}