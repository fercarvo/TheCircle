using System;
using System.ComponentModel.DataAnnotations;

namespace TheCircle.Models
{
    public class Reporte
    {

        public class Egreso
        {
            [Key]
            public Int64 cont { get; set; }
            public int idItem { get; set; }
            public string nombre { get; set; } = null;
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

        public class Ingreso
        {
            [Key]
            public int idItem { get; set; }
            public string nombre { get; set; } = null;
            public string compuesto { get; set; }
            public string categoria { get; set; }
            public string grupo { get; set; }
            public DateTime fcaducidad { get; set; }
            public string localidad { get; set; }
            public string tipo { get; set; }
            public int cantidad { get; set; }
            public string personal { get; set; }
            public DateTime fecha { get; set; }
        }
    }
}
