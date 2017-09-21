using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class ItemReceta
    {
        [Key]
        public int id { get; set; }
        public int idItemFarmacia { get; set; }
        public DateTime? fcaducidad { get; set; }
        public string nombre { get; set; }
        public string compuesto { get; set; }
        public Int32 diagnostico { get; set; }
        public int cantidad { get; set; }
        public string posologia { get; set; }
        public Boolean? funciono { get; set; }

        public ItemReceta() { }

        public ItemReceta (int receta, Data i, MyDbContext _context) {
            try {
                var q = $"EXEC ItemReceta_Insert @idItemFarmacia={i.itemFarmacia.id}" +
                $", @idDiagnostico={i.diagnostico}, @cantidad={i.cantidad}" +
                $", @receta={receta}, @posologia='{i.posologia}'";

                _context.Database.ExecuteSqlCommand(q); 
            } catch (Exception e) {
                throw new Exception("Error al insertar ItemReceta", e);
            }            
        }

        public static ItemReceta[] ReportReceta( int receta, MyDbContext _context) 
        {
            string q = $"EXEC ItemReceta_Report_Receta @receta={receta}";
            return _context.ItemsReceta.FromSql(q).ToArray();
        }

        public class Data
        {
            [Key]
            public ItemFarmacia itemFarmacia { get; set; }
            public string diagnostico { get; set; }
            public int cantidad { get; set; }
            public string posologia { get; set; }
        }

    }
}
