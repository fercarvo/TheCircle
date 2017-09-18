using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class Categoria
    {
        [Key]
        public int id { get; set; }
        public string nombre { get; set; }

        public Categoria() { }

        public static Categoria[] Report()
        {
            var data = new MyDbContext().Categoria.FromSql("EXEC Categoria_Report").ToArray();
            return data;
        }
    }
}
