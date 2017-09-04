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
        public DateTime fcaducidad { get; set; }
        public string nombre { get; set; }
        public string compuesto { get; set; }
        public Int32 diagnostico { get; set; }
        public int cantidad { get; set; }
        public string posologia { get; set; }
        public Boolean? funciono { get; set; }

        public ItemReceta() { }

        public ItemReceta[] insert(int receta, ItemRecetaRequest[] items, MyDbContext _context)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                foreach (ItemRecetaRequest item in items) //se insertan en la base de datos todos los items
                    insertItem(receta, item, _context);

                transaction.Commit();

                return getAllByReceta(receta, _context);

            } catch {
                transaction.Rollback();
                throw new Exception("Error al insertar los items de Receta at ItemReceta.insert");
            }            
        }

        private void insertItem (int receta, ItemRecetaRequest i, MyDbContext _context) {
            string query = $"EXEC dbo.insert_ItemReceta @idItemFarmacia={i.itemFarmacia.id}" +
                $", @idDiagnostico={i.diagnostico}" +
                $", @cantidad={i.cantidad}" +
                $", @receta={receta}" +
                $", @posologia='{i.posologia}'";

             _context.Database.ExecuteSqlCommand(query);
        }

        public ItemReceta[] getAllByReceta(int receta, MyDbContext _context) {
            try {
                var data = _context.ItemsReceta.FromSql($"EXEC dbo.select_ItemRecetaByReceta @receta={receta}").ToArray();
                return data;
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

    /*
    public class RecetaItemsRequest
    {
        public int idReceta { get; set; }
        public ItemRecetaRequest[] items { get; set; }

        public RecetaItemsRequest() { }
    }
    */
}
