

namespace TheCircle.Models
{
    public class Diagnostico
    {

        public Diagnostico(string e, int a)
        {
            this.enfermedad = e;
            this.atencionM = a;
        }

        public string enfermedad;
        public int atencionM;
    }
}
