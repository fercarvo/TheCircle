using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class ItemDespacho 
    {
        [Key]
        public int id { get; set; }
        public string nombreItem { get; set; }
        public string compuesto { get; set; }
        public DateTime fecha { get; set; }
        public int cantidadDespachada { get; set; }
        public int cantidadRecetada { get; set; }
        public string comentario { get; set; }
        public string idPersonal { get; set; }

        public ItemDespacho () {}

        public ItemDespacho (Data item, int personal, MyDbContext _context) {
            try {
                string q = $"EXEC DespachoReceta_Insert @id_itemReceta={item.itemReceta}, @cantidad={item.cantidad}, @personal={personal}, @comentario='{item.comentario}'";
                _context.Database.ExecuteSqlCommand(q);
                
            } catch (Exception e) {
                throw new Exception ("No se pudo ingresar el ItemDespacho", e);
            }            
        }

        //Obtengo todos los Despachos de esa receta
        public static ItemDespacho[] GetByReceta(int receta, MyDbContext _context)
        {
            string q = $"EXEC DespachoReceta_Report_ByReceta @idReceta={receta}";
            return _context.ItemDespacho.FromSql(q).ToArray();
        }


        public class Data
        {
            public int itemReceta { get; set; }
            public int cantidad { get; set; }
            public string comentario { get; set; }
        }
    }
}
