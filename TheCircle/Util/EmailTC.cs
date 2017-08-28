using System;
//using System.Net.Mail;
using System.Net;

namespace TheCircle.Util
{
    public class EmailTC
    {   
        /*
        private Smtp smtp2;
        private SmtpClient smtp;
        private MailAddress sender;
        private MailAddress receiver;

        public EmailTC(string email){
            var sender = new MailAddress("thecircle@children.org.ec", "TheCircle Children International GUY");
            var receiver = new MailAddress(email, "");
            string password = "blabla";
            var smtp = new SmtpClient {
                Host = "smtp.children.org.ec",
                Port = 587,
                //EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(sender.Address, password),
                Timeout = 20000
            };

            this.smtp = smtp;
            this.sender = sender;
            this.receiver = receiver;
        }

        public send (string subject, string body) {
            var message = new MailMessage(this.sender, this.receiver) {
              Subject = subject,
              Body = body
            }
            this.smtp.Send(message);
        }*/

    }



}
