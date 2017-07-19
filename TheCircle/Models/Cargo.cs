

using System.ComponentModel.DataAnnotations;

namespace TheCircle.Models
{
    public class Cargo
    {
        [Key]
        public string tipo { get; set; }

        public Cargo() {
        }

       
        //public int atencionM;
    }
}
