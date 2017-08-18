
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TheCircle.Models
{
    public class RecetaDespacho {
        public int idReceta { get; set; }
        public ItemDespacho[] items { get; set; }
 
    }

    public class ItemDespacho {
        [Key]
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public int cantidadDespachada { get; set; }
        public int cantidadRecetada { get; set; }
        public string comentario { get; set; }
        public int idPersonal { get; set; }

        public ItemDespacho[] getByReceta(int idReceta, MyDbContext _context)
        {
            string q = $"EXEC dbo.select_DespachoRecetaByReceta @idReceta={idReceta}";
            try {
                var recetas =  _context.ItemDespacho.FromSql(q).ToArray();
                return recetas;
            } catch (Exception e) {
                return null;
            }
        }
    }

    public class DespachoRecetaRequest
    {
        public int id { get; set; }
        public ItemsDespachoRequest[] items { get; set; }

        public DespachoRecetaRequest() { }

    }

    public class ItemsDespachoRequest
    {
        public int itemReceta { get; set; }
        public int cantidad { get; set; }
        public int personal { get; set; }
        public string comentario { get; set; }

        public void insert(ItemsDespachoRequest item, MyDbContext _context) {
            string q = $"EXEC dbo.insert_DespachoReceta @id_itemReceta={item.itemReceta}, @cantidad={item.cantidad}, @personal={item.personal}, @comentario='{item.comentario}'";
            try {
                _context.Database.ExecuteSqlCommand(q);
            } catch (Exception e) {
            }
        }
    }
}
