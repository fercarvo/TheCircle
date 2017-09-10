using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class Institucion
    {
        [Key]
        public int id { get; set; }
        public string nombre { get; set; }
        public string especialidad { get; set; }
        public string genero { get; set; }
        public double costo { get; set; }
        public int edadInicial { get; set; }
        public int edadFinal { get; set; }

        public Institucion() { }

        public Institucion[] Report(MyDbContext _context) 
        {
            var data = _context.Instituciones.FromSql("EXEC dbo.select_Institucion").ToArray();
            return data;
        }
    }
}
