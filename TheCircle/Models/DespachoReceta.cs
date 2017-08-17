
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace TheCircle.Models
{
    public class RecetaDespacho {
        public Receta receta { get; set; }
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
            string q = $"EXEC dbo.insert_DespachoReceta @itemReceta={item.itemReceta}, @cantidad={item.cantidad}, @personal={item.personal}, @comentario='{item.comentario}'";
            try {
                _context.Database.ExecuteSqlCommand(q);
            } catch (Exception e) {
            }
        }
    }
}
