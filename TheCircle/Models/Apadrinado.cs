using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheCircle.Models
{
    public class Apadrinado
    {
        [Key]
        public int codigo { get; set; }
        public string idLocalidad { get; set; }
        public string nombreLocalidad { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }

        public Apadrinado() { }
    }
}
