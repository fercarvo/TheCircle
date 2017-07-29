
using System;
using System.Collections.Generic;

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

    public class Atencion2
    {
        public Atencion atencion { get; set; }
        public List<Diagnostico> diagnosticos { get; set; }

        public Atencion2() { }
    }

    public class Diagnostico
    {
        public int id { get; set; }
        public string enfermedadCod { get; set; }
        public string enfermedadNombre { get; set; }

        public Diagnostico() { }

        public Diagnostico(int id, string enfermedadCod, string enfermedadNombre)
        {
            this.id = id;
            this.enfermedadCod = enfermedadCod;
            this.enfermedadNombre = enfermedadNombre;
        }
    }
}
