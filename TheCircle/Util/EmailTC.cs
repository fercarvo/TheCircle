using MailKit.Net.Smtp;
using MimeKit;
using System;

namespace TheCircle.Util
{
    public class EmailTC
    {
        private MailboxAddress from;
        private SmtpClient client;
        private string userName;
        private string password;
        private string host;
        private int port;

        public EmailTC()
        {
            from = new MailboxAddress("The Circle", "notificaciones@guy.children.org.ec");
            userName = "notificaciones@guy.children.org.ec";
            password = "P@ssw0rd2014";
            host = "199.167.147.65";
            port = 587;
            client = new SmtpClient();

            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            client.Connect(host, port, false);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(userName, password);
        }

        public void NuevaClave(string nombre, string email, string clave) {
            try {
                var message = new MimeMessage();
                message.From.Add(from);
                message.To.Add(new MailboxAddress(nombre, "ecarvajal@guy.children.org.ec")); //Cambiar por nombre e email
                message.Subject = "Recuperacion de contrase�a";

                message.Body = new TextPart("plain")
                {
                    Text = @"The Circle le saluda,

Usted recientemente realiz� una solicitud de recuperaci�n de contrase�a,
Su contrase�a generada es: " + $"{clave}" + @"

Por favor cambie la misma lo antes posible.

TheCircle, CI"
                };

                client.Send(message);
                client.Disconnect(true);
            } catch (Exception e) {
                return;
            }            
        }

        public void CambiarClave(string nombre, string email, string clave)
        {
            try {
                var message = new MimeMessage();
                message.From.Add(from);
                message.To.Add(new MailboxAddress(nombre, "ecarvajal@guy.children.org.ec")); //Cambiar por nombre e email
                message.Subject = "Cambio de contrase�a";

                message.Body = new TextPart("plain")
                {
                    Text = @"The Circle le saluda,

Usted recientemente realiz� un cambio de contrase�a, en caso de no haberlo hecho comun�quese con sistemas.

TheCircle, CI"
                };

                client.Send(message);
                client.Disconnect(true);
            } catch (Exception e) {
                return;
            }            
        }
    }
}
