using System.ComponentModel.DataAnnotations;


namespace TheCircle.Models
{
    public class Institucion
    {
        [Key]
        public int id { get; set; }
        public string nombre { get; set; }
        public string especialidad { get; set; }
        public string genero { get; set; }
        public double costo { get; set; }
        public int edadInicial { get; set; }
        public int edadFinal { get; set; }

        public Institucion() { }
    }
}
