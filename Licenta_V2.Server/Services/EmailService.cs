using System.Net;
using System.Net.Mail;

namespace LatissimusDorsi.Server.Services
{
    public class EmailService
    {
        private string _email="rares.cojocaru@student.tuiasi.ro";
        private string _password = "rhcy plsz urgv bcht";

        public void SendPdf(string receiver, string filePath)
        {
            MailMessage mail = new MailMessage(_email, receiver);
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");

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
