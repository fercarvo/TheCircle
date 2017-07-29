
namespace TheCircle.Models
{
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
