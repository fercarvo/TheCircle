using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using TheCircle.Models;

namespace TheCircle.Util
{
    public enum Localidad { HEE, CC0, CC2, CC3, CC5, CC6, OC }

    public class Token
    {
        public Data data { get; set; }
        public string sign { get; set; }

        public Token(User user, Localidad loc) {

            Data data = new Data(user, loc);
            Signature _signer = new Signature();
            string newDataToString = JsonConvert.SerializeObject(data);

            this.data = data;
            this.sign = _signer.sign_HMAC(newDataToString);
        }

        public Token() { }

        /*  @request: el requerimiento de donde se extraerá el cookie de session
            @cargo: El cargo que deberia tener el cookie de session
            En caso de no cumplir alguna validacion, se dispara un exception
        */
        internal Token check(HttpRequest request, string cargo) {
            try {
                string cookieSession = request.Cookies["session"]; //Se obtiene el string de la cookie
                Signature _signer = new Signature();
                Token token;

                if (string.IsNullOrEmpty(cookieSession) || string.IsNullOrEmpty(cookieSession))
                    throw new TokenException("No existe cookieSession/cargo, at Token.check");

                token = JsonConvert.DeserializeObject<Token>(cookieSession); //Se parcea el string de cookie a Token.

                if (token.data.expireAt < DateTime.Now)
                    throw new TokenException("Token expirado, at Token.check");

                if (token.data.cargo != cargo)
                    throw new TokenException("Cargo incorrecto, no autorizado, at Token.check");

                string dataToString = JsonConvert.SerializeObject(token.data);

                if (_signer.checkHMAC(dataToString, token.sign) == false)
                    throw new TokenException("Alerta, Token alterado, at Token.check"); //Se validara cuando un tercero intente hackear el sistema

                return token;

            } catch (Exception e) {
                throw new TokenException("Algo salio mal at Token.check");
            }
        }

    }

    public class Data
    {
        public int cedula { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string cargo { get; set; }
        public Localidad localidad { get; set; }
        public DateTime issueAt { get; set; }
        public DateTime expireAt { get; set; }

        public Data() { }

        public Data(User usr, Localidad lc) {
            cedula = usr.cedula;
            nombres = usr.nombre;
            apellidos = usr.apellido;
            cargo = usr.cargo;
            localidad = lc;
            issueAt = DateTime.Now;
            expireAt = DateTime.Now.AddHours(10);
        }
    }


    public class LoginRequest
    {
        [BindRequired]
        [StringLength(10)]
        public string cedula { get; set; }
        [BindRequired]
        [StringLength(10)]
        public string clave { get; set; }
        [BindRequired]
        public Localidad localidad { get; set; }
    }

    public class LoginMessage
    {
        [BindRequired]
        public int flag { get; set; }
        [BindRequired]
        [StringLength(120)]
        public string msg { get; set; }
    }
}
