using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class UnidadMedida
    {
        [Key]
        public string nombre { get; set; }

        public UnidadMedida() { }

        public static UnidadMedida[] Report()
        {
            var data = new MyDbContext().UnidadMedida.FromSql("EXEC Unidad_Report").ToArray();
            return data;
        }
    }
}