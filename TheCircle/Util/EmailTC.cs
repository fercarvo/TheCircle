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
                message.Subject = "Recuperacion de contraseña";

                message.Body = new TextPart("plain")
                {
                    Text = @"The Circle le saluda,

Usted recientemente realizó una solicitud de recuperación de contraseña,
Su contraseña generada es: " + $"{clave}" + @"

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
                message.Subject = "Cambio de contraseña";

                message.Body = new TextPart("plain")
                {
                    Text = @"The Circle le saluda,

Usted recientemente realizó un cambio de contraseña, en caso de no haberlo hecho comuníquese con sistemas.

TheCircle, CI"
                };

                client.Send(message);
                client.Disconnect(true);
            } catch (Exception e) {
                return;
            }            
        }

        public void TransferenciaErronea(string nombre, string email, int transferencia)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(from);
                message.To.Add(new MailboxAddress(nombre, "ecarvajal@guy.children.org.ec")); //Cambiar por nombre e email
                message.Subject = "Despacho de transferencia inconsistente";

                message.Body = new TextPart("plain")
                {
                    Text = @"The Circle le saluda,

Recientemente se despacho una transferencia con una cantidad inferior al solicitado, el número de la misma es " + $"0000{transferencia}"
                };

                client.Send(message);
                client.Disconnect(true);
            }
            catch (Exception e)
            {
                return;
            }
        }

        public void RecetaErronea(string nombre, string email, int receta)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(from);
                message.To.Add(new MailboxAddress(nombre, "ecarvajal@guy.children.org.ec")); //Cambiar por nombre e email
                message.Subject = "Despacho de receta inconsistente";

                message.Body = new TextPart("plain")
                {
                    Text = @"The Circle le saluda,

Recientemente se despacho una receta médica con una cantidad inferior a los items solicitados, el número de la misma es " + $"0000{receta}"
                };

                client.Send(message);
                client.Disconnect(true);
            }
            catch (Exception e)
            {
                return;
            }
        }

        public void PedidoInternoErroneo(string nombre, string email, int pedido)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(from);
                message.To.Add(new MailboxAddress(nombre, "ecarvajal@guy.children.org.ec")); //Cambiar por nombre e email
                message.Subject = "Despacho de receta inconsistente";

                message.Body = new TextPart("plain")
                {
                    Text = @"The Circle le saluda,

Recientemente se despacho un pedido interno con una cantidad inferior a la solicitada, el número del mismo es " + $"0000{pedido}"
                };

                client.Send(message);
                client.Disconnect(true);
            }
            catch (Exception e)
            {
                return;
            }
        }

        public void AlertaPesoTalla(string nombre, string email, int codigo, int? peso, int? talla, string doctor)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(from);
                message.To.Add(new MailboxAddress(nombre, "ecarvajal@guy.children.org.ec")); //Cambiar por nombre e email
                message.Subject = "Alerta de peso y talla";

                message.Body = new TextPart("plain")
                {
                    Text = @"The Circle le saluda,

Por favor considerar el siguiente valor de peso y talla para su revisión en la base de datos Aptify

El doctor " + doctor + @", ha registrado " + $"PESO: {peso}kg, TALLA: {talla}cm, para el código: {codigo}, gracias." 
                };

                client.Send(message);
                client.Disconnect(true);
            }
            catch (Exception e)
            {
                return;
            }
        }
    }
}
