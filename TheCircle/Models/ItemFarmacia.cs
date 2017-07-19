using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheCircle.Models
{
    public class ItemFarmacia
    {
        [Key]
        public int id { get; set; }
        public string nombre { get; set; }
        public string compuesto { get; set; }
        public int stock { get; set; }
        public DateTime fcaducidad { get; set; }
        public DateTime fregistro { get; set; }
        public string localidad { get; set; }
        public int personal { get; set; }

        public ItemFarmacia() { }
    }
}
