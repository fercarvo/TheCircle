using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheCircle.Models
{
    public class Remision
    {
        [Key]
        public int id { get; set; }
        public int atencionM { get; set; }
        public int doctor { get; set; }
        public string institucion { get; set; }
        public double monto { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fCaducidad { get; set; }

        public Remision() { }

        /*
        public Remision(int id, int atencionM, int doctor, string institucion, double monto, int fecha, int fCaducidad)
        {
            this.id = id;
            this.atencionM = atencionM;
            this.doctor = doctor;
            this.institucion = institucion;
            this.monto = monto;
            this.fecha = fecha;
            this.fCaducidad = fCaducidad;
        }*/
    }
}
