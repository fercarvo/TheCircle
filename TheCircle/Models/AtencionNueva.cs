using System;

namespace TheCircle.Models
{
    public class AtencionNueva
    {
        public int doctor { get; set; }
        public int apadrinado { get; set; }
        public string tipo { get; set; }
        public string diagnosticop { get; set; }
        public string diagnostico1 { get; set; }
        public string diagnostico2 { get; set; }

        public AtencionNueva() { }
    }
}
