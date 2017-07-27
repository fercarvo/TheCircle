using System;
using System.ComponentModel.DataAnnotations;


namespace TheCircle.Models
{
    public class ItemFarmacia
    {
        [Key]
        public int id { get; set; }
        public string nombre { get; set; }
        public string compuesto { get; set; }
        public string categoria { get; set; }
        public string grupo { get; set; }
        public int stock { get; set; }
        public DateTime fcaducidad { get; set; }

        public ItemFarmacia() { }
    }
}
