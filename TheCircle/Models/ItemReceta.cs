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

        /*
            Metodo que recibe una lista de items y se los inserta en la BDD
            En caso de haber un error por datos incorrectos o cualquier cosa, se hace rollback
        */
        public void insert(int receta, ItemRecetaRequest[] items, MyDbContext _context)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                foreach (ItemRecetaRequest item in items)
                    insertItem(receta, item, _context);

                transaction.Commit();
                //return getAllByReceta(receta, _context);
            } catch {
                transaction.Rollback();
                throw new Exception("Error al insertar los items de Receta at ItemReceta.insert");
            }            
        }

        /*
            Recibe un id de Receta y un ItemRequest, se los inserta en la BDD
        */
        private void insertItem (int receta, ItemRecetaRequest i, MyDbContext _context) 
        {
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
}
