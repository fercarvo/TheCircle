using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using TheCircle.Models;
using System.Linq;

namespace TheCircle.Util
{
    public enum Localidad { HEE, CC0, CC2, CC3, CC5, CC6, OC }

    public class Token
    {
        public Data data { get; set; }
        public string sign { get; set; }

        public Token(Usuario usuario, Localidad localidad) 
        {
            data = new Data(usuario, localidad);

            string data_String = JsonConvert.SerializeObject(data);

            sign = Signature.Sign(data_String);
        }

        public Token() { }



        /*  @request: el requerimiento de donde se extraerá el cookie de session
            @cargo: El cargo que deberia tener el cookie de session
            En caso de no cumplir alguna validacion, se dispara un exception
        */
        internal static Token Check(HttpRequest request, string[] cargos) {
            string cookie = request.Cookies["session"]; //Se obtiene el string de la cookie
            Token token;

            if (string.IsNullOrEmpty(cookie))
                throw new TokenException("No existe cookie/cargo, at Token.check");

            //token = JsonConvert.DeserializeObject<Token>(cookie); //Se parcea el string de cookie a Token.
            token = JsonConvert.DeserializeObject<Token>(Signature.FromBase(cookie)); //Se parcea el string de cookie a Token.

            if (token.data.expireAt < DateTime.Now)
                throw new TokenException("Token expirado, at Token.check");

            if(!cargos.Contains(token.data.cargo))
                throw new TokenException("Cargo incorrecto, no autorizado, at Token.check");

            string dataToString = JsonConvert.SerializeObject(token.data);

            Signature.CheckHMAC(dataToString, token.sign);

            return token;
        }

    }

    public class Data
    {
        public int cedula { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string cargo { get; set; }
        public string email { get; set; }
        public Localidad localidad { get; set; }
        public DateTime issueAt { get; set; }
        public DateTime expireAt { get; set; }

        public Data() { }

        public Data(Usuario usr, Localidad lc) {
            cedula = usr.cedula;
            nombres = usr.nombre;
            apellidos = usr.apellido;
            cargo = usr.cargo;
            email = usr.email;
            localidad = lc;
            issueAt = DateTime.Now;
            expireAt = new DateTime(issueAt.Year, issueAt.Month, issueAt.Day, 22, 0, 0); 
        }
    }


    public class LoginRequest
    {
        [BindRequired]
        [StringLength(10)]
        public string cedula { get; set; }
        [BindRequired]
        [StringLength(20)]
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
