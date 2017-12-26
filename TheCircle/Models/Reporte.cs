using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class Reporte
    {

        public class Egreso
        {
            [Key]
            public int idItem { get; set; }
            public string nombre { get; set; }
            public string compuesto { get; set; }
            public string categoria { get; set; }
            public string grupo { get; set; }
            public DateTime fcaducidad { get; set; }
            public string localidad { get; set; }
            public string tipo { get; set; }
            public int cantidadSolicitada { get; set; }
            public int cantidadDespachada { get; set; }
            public DateTime fDespacho { get; set; }
            public string personal { get; set; }
        }
    }
}
