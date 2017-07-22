using System;
using System.ComponentModel.DataAnnotations;

namespace TheCircle.Models
{
    public class Apadrinado
    {
        [Key]
        public string id { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public Int16 income { get; set; }
        public string sector { get; set; }
        public decimal edad { get; set; }
        public int numPer { get; set; }
        public int numBeds { get; set; }
        public string posesionHogar { get; set; }

        public Apadrinado() { }
    }
}
