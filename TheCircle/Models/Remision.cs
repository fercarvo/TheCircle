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
        public string IdInstitucion { get; set; }
        public string nombreInstitucion { get; set; }
        public double monto { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fCaducidad { get; set; }

        public Remision() { }
    }
}
