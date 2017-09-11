
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class RecetaDespacho 
    {
        [Key]
        public Receta receta { get; set; }
        public ItemDespacho[] items { get; set; }

        public RecetaDespacho(Receta receta, ItemDespacho[] items)
        {
            this.receta = receta;
            this.items = items;
        }

        public RecetaDespacho() { }

        public List<RecetaDespacho> getBy_Asistente (int asistente, MyDbContext _context) 
        {
            ItemDespacho i = new ItemDespacho();

            Receta[] recetas = new Receta().getBy_Asistente(asistente, _context);
            List<RecetaDespacho> recetasDespacho = new List<RecetaDespacho>();

            foreach (Receta receta in recetas) {
                ItemDespacho[] items = i.getByReceta(receta.id, _context);
                if (items != null) 
                    recetasDespacho.Add(new RecetaDespacho(receta, items));
            }
            return recetasDespacho;
        }
    }

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
        public int idPersonal { get; set; }

        public ItemDespacho[] getByReceta(int idReceta, MyDbContext _context)
        {
            string q = $"EXEC dbo.DespachoReceta_Report_ByReceta @idReceta={idReceta}";
            try {
                var data = _context.ItemDespacho.FromSql(q).ToArray();
                return data;
            } catch (Exception e) {
                return null;
            }
        }

        public static void Insert(int receta, ItemsDespachoRequest[] items, int personal, MyDbContext _context)
        {
            var transaction = _context.Database.BeginTransaction();
            try {
                
                foreach (ItemsDespachoRequest item in items) //se insertan en la base de datos todos los items
                    InsertItem(item, personal, _context);

                Receta.UpdateDespachada(receta, _context);
                transaction.Commit();

            } catch (Exception e) {
                transaction.Rollback();
                throw new Exception("Error al insertar los despacho de items at ItemDespacho.insert");
            }
        }

        static void InsertItem(ItemsDespachoRequest item, int personal, MyDbContext _context)
        {
            string q = $"EXEC dbo.DespachoReceta_Insert @id_itemReceta={item.itemReceta}, @cantidad={item.cantidad}, @personal={personal}, @comentario='{item.comentario}'";
            _context.Database.ExecuteSqlCommand(q);
        }
    }

    public class ItemsDespachoRequest
    {
        public int itemReceta { get; set; }
        public int cantidad { get; set; }
        public string comentario { get; set; }
    }
}
