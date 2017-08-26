using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

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

        public ItemFarmacia[] getAllByLocalidad (string localidad, MyDbContext _context)
        {
            string query = $"EXEC dbo.select_ItemFarmacia @localidad={localidad}";

            try {
                var data = _context.ItemFarmacias.FromSql(query).ToArray();
                return data;
            } catch (Exception e) {
                Console.WriteLine(e);
                throw new Exception("Error cargar ItemsFarmacia, ItemFarmacia.getAllByLocalidad");
            }
        }
    }
}
