using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace TheCircle.Models
{
    public class Login
    {

    }

    public class Token
    {
        public Token(Data payload, string sign)
        {
            this.payload = payload;
            this.sign = sign;
        }

        public Data payload { get; set; }
        public string sign { get; set; }

        public override string ToString()
        {
            return $"{{'payload':'{this.payload.ToString()}','sign':'{this.sign}'}}";
        }

    }

    public class Data
    {
        public string cedula { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string cargo { get; set; }
        public string localidad { get; set; }
        public string path { get; set; }
        public DateTime issueAt { get; set; }
        public DateTime expireAt { get; set; }

        public override string ToString()
        {
            return $"{{'cedula':'{this.cedula}','nombres':'{this.nombres}','apellidos':'{this.apellidos}','issueAt':'{this.issueAt}'}}";
        }
    }

    public class LoginRequest
    {
        [BindRequired]
        public string cedula { get; set; }
        [BindRequired]
        public string clave { get; set; }
        [BindRequired]
        public string localidad { get; set; }
    }



}
