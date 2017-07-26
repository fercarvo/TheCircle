using System;

namespace TheCircle.Models
{
    public class AtencionNueva
    {
        public int doctor { get; set; }
        public int apadrinado { get; set; }
        public string tipo { get; set; }
        public string diagp { get; set; }
        public string diag1 { get; set; }
        public string diag2 { get; set; }

        public AtencionNueva() { }
    }
}
