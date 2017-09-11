using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;
using TheCircle.Controllers;

namespace TheCircle.Models
{
    public class ItemFarmacia
    {
        [Key]
        public int id { get; set; }
        public string nombre { get; set; }
        public string compuesto { get; set; }
        public string categoria { get; set; }
        public string grupo { get; set; }
        public int stock { get; set; }
        public DateTime? fcaducidad { get; set; }

        public ItemFarmacia() { }

        public static ItemFarmacia[] ReportLocalidad (Localidad localidad, MyDbContext _context)
        {
            string query = $"EXEC dbo.select_ItemFarmacia @localidad='{localidad}'";

            var data = _context.ItemFarmacias.FromSql(query).ToArray();
            return data;
        }

        public static void New(Ingreso item, Localidad localidad, int personal, MyDbContext _context) 
        {
            string query = $"EXEC dbo.ItemFarmacia_insert @nombre='{item.nombre}', @compuesto='{item.compuesto}', @fcaducidad='{item.fcaducidad}', @cantidad={item.cantidad}, @localidad='{localidad}', @personal={personal}";
            _context.Database.ExecuteSqlCommand(query);
        }

        internal static void New(IngresoTransferencia it, Localidad localidad, int personal, MyDbContext _context)
        {
            string query = $"EXEC ItemFarmacia_Insert_Transferencia @idTransferencia={it.idTransferencia}, @comentario='{it.comentario}', @localidad='{localidad}', @personal={personal}";
            _context.Database.ExecuteSqlCommand(query);
        }

        public class IngresoTransferencia
        {
            public int idTransferencia { get; set; }
            public string comentario { get; set; }

            public IngresoTransferencia() { }
        }

        public class Ingreso
        {
            public string nombre { get; set; }
            public string compuesto { get; set; }
            public string fcaducidad { get; set; }
            public int cantidad { get; set; }

            public Ingreso() { }
        }
    }
}
