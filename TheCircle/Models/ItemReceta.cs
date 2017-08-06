using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TheCircle.Models
{
    public class ItemReceta
    {
        [Key]
        public int id { get; set; }
        public int idItemFarmacia { get; set; }
        public Int32 diagnostico { get; set; }
        public int cantidad { get; set; }
        public int receta { get; set; }
        public DateTime fecha { get; set; }
        public string posologia { get; set; }
        public int? funciono { get; set; }

        public ItemReceta() { }

        public void insert (int receta, ItemRecetaRequest i, MyDbContext _context) {
            string query = $"EXEC dbo.insert_ItemReceta @idItemFarmacia={i.itemFarmacia.id}" +
              $", @idDiagnostico={i.diagnostico}" +
              $", @cantidad={i.cantidad}" +
              $", @receta={receta}" +
              $", @posologia='{i.posologia}'";

              try {
                  _context.Database.ExecuteSqlCommand(query);
              } catch (Exception e) {
              }
        }

        public ItemReceta[] getAllByReceta(int receta, MyDbContext _context) {
            try {
                var data = _context.ItemsReceta.FromSql($"EXEC dbo.select_ItemRecetaByReceta @receta={receta}");
                return data.ToArray();
            } catch (Exception e) {
                return null;
            }
        }
    }

    public class ItemRecetaRequest
    {
        [Key]
        public ItemFarmacia itemFarmacia { get; set; }
        public string diagnostico { get; set; }
        public int cantidad { get; set; }
        public string posologia { get; set; }

        public ItemRecetaRequest() { }
    }

    public class RecetaItemsRequest
    {
        public int idReceta { get; set; }
        public ItemRecetaRequest[] items { get; set; }

        public RecetaItemsRequest() { }
    }
}
