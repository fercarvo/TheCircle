

using System.ComponentModel.DataAnnotations;

namespace TheCircle.Models
{
    public class Foto
    {
        [Key]
        public string name { get; set; }
        public string path { get; set; }

        public Foto() { }
    }
}
