using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tools
{
    public static class EmailSender
    {
        public static async Task<bool> SendEmail(string to, string subject, string body)
        {
            string email = "awref.ahngar@gmail.com";
            string password = "nfevaamqluuiainw";

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(email, "فروشگاه ما");
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpServer.Port = 587;
                SmtpServer.EnableSsl = true;


                SmtpServer.Credentials = new System.Net.NetworkCredential(email, password);
                SmtpServer.Send(mail);

                return true;
            }
            catch (Exception exception)
            {
                return false;
            }
        }
    }
}
