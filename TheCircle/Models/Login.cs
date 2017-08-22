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
        public string cedula { get; set; }
        public string clave { get; set; }
    }



}
