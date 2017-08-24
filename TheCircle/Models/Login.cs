using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace TheCircle.Models
{
    public class Login
    {

    }

    public class Token
    {
        public Data payload { get; set; }
        public string sign { get; set; }

    }

    public class Data
    {
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string cargo { get; set; }
        public string localidad { get; set; }
        public DateTime issueAt { get; set; }
        public DateTime expireAt { get; set; }
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
