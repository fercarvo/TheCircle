using System.ComponentModel.DataAnnotations;


namespace TheCircle.Models
{
    public class Enfermedad
    {
        [Key]
        public string codigo { get; set; }
        public string nombre { get; set; }

        public Enfermedad() { }
    }
}
