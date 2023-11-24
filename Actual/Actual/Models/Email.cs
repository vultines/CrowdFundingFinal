using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Net;
using System.Net.Mail;

namespace Actual.Models
{
    public class Email
    {

        public string emails = "maribelmita4@gmail.com";
        public string pass = "lcosxrawihoyvznz";

        public bool EnviarToken(string token, string toemail)
        {
            try
            {
                var fromAddress = new MailAddress(emails);
                var toAddress = new MailAddress(toemail);
                string subjet = "Restablecimiento de contraseña"; // Línea de asunto descriptiva
                string resetLink = "https://localhost:7029/Account/ResetPassword?token=" + token;
                string body = "Hola,\n\nRecibimos una solicitud para restablecer la contraseña asociada a esta dirección de correo electrónico. Si no realizaste esta solicitud, ignora este correo electrónico.\n\nPara restablecer tu contraseña, ingresa el siguiente código en la página de restablecimiento de contraseña: " + token + "\n\nTambién puedes hacer clic en el siguiente enlace para restablecer tu contraseña: " + resetLink + "\n\nAtentamente,\nEl equipo de soporte";



                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage email = new MailMessage();



                email.From = fromAddress;
                email.To.Add(toAddress);
                email.Subject = subjet;
                email.Body = body;



                SmtpServer.Timeout = 5000;
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(emails, pass);
                SmtpServer.Send(email);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
