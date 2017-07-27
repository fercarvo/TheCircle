
using System;

namespace TheCircle.Models
{
    public class Atencion
    {
        public int id { get; set; }
        public string idApadrinado { get; set; }
        public string nombreApadrinado { get; set; }
        public int idDoctor { get; set; }
        public string nombreDoctor { get; set; }
        public DateTime fecha { get; set; }
        public double? peso { get; set; }
        public double? talla { get; set; }
        public string tipo { get; set; }

        public Atencion () { }
    }
}
