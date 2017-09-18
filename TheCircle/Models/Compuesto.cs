using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{

    public class Compuesto
    {
        [Key]
        public string nombre { get; set; }
        public string unidadMedida { get; set; }
        public string categoriaNombre { get; set; }
        public string categoriaCodigo { get; set; }
        public string grupo { get; set; }

        public Compuesto() { }


        public Compuesto(string nombre, int categoria, string unidad) {
            try {
                string q = $"EXEC Compuesto_Insert @nombre='{nombre}', @categoria={categoria}, @unidad='{unidad}'";
                new MyDbContext().Database.ExecuteSqlCommand(q);

            } catch (Exception e) {
                throw new Exception("No se pudo crear el compuesto", e);
            }            
        }

        public static Compuesto[] Report() 
        {
            var data = new MyDbContext().Compuesto.FromSql("EXEC Compuesto_Report").ToArray();
            return data;
        }

        public class Data
        {
            public string nombre { get; set; }
            public int categoria { get; set; }
            public string unidad { get; set; }
        }
    }
}
