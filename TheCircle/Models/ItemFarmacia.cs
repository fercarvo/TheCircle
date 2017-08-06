using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        public ItemFarmacia[] getAllByLocalidad (string localidad, MyDbContext _context) {

            try {
                string query = $"EXEC dbo.select_ItemFarmacia @localidad={localidad}";
                return _context.ItemFarmacias.FromSql(query).ToArray();
            } catch (Exception e) {
                return null;
            }
        }
    }
}
