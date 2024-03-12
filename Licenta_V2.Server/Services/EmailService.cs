using System.Net;
using System.Net.Mail;

namespace LatissimusDorsi.Server.Services
{
    public class EmailService
    {
        private string _email= "LatissimusDorsi.NET@outlook.com";
        private string _password = "lvwezoelkrqgbxgz";

        public void SendPdf(string receiver, string filePath)
        {
            MailMessage mail = new MailMessage(_email, receiver);
            SmtpClient smtpClient = new SmtpClient("smtp.outlook.com");

            mail.Subject = "Workout Program";
            mail.Body = "Your workout program has arrived !";

            mail.Attachments.Add(new Attachment(filePath));

            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(_email, _password);
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while sending email: " + ex.Message);
            }
            finally
            {
                smtpClient.Dispose();
                mail.Dispose();
                File.Delete(filePath);
            }

        }

    }
}
