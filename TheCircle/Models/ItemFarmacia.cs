using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

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
        public string localidad { get; set; }

        public ItemFarmacia() { }

        public ItemFarmacia(Ingreso item, Localidad localidad, int personal) {
            try {
                string query = $"EXEC ItemFarmacia_insert @nombre='{item.nombre}', " +
                    $"@compuesto='{item.compuesto}', " +
                    $"@fcaducidad='{item.fcaducidad}', " +
                    $"@cantidad={item.cantidad}, " +
                    $"@localidad='{localidad}', " +
                    $"@personal={personal}";

                new MyDbContext().Database.ExecuteSqlCommand(query);

                nombre = item.nombre;
                compuesto = item.compuesto;
                stock = item.cantidad;

            } catch (Exception e) {
                throw new Exception("No se pudo crear el itemFarmacia", e);
            }
        }

        public ItemFarmacia(int idTransferencia, string comentario, Localidad localidad, int personal) {
            try {
                string query = $"EXEC ItemFarmacia_Insert_Transferencia @idTransferencia={idTransferencia}, " +
                    $"@comentario='{comentario}', " +
                    $"@localidad='{localidad}', " +
                    $"@personal={personal}";

                new MyDbContext().Database.ExecuteSqlCommand(query);

            } catch (Exception e) {
                throw new Exception("No se pudo crear el itemFarmacia", e);
            }
        }

        public static ItemFarmacia[] ReportLocalidad (Localidad localidad, MyDbContext _context)
        {
            string query = $"EXEC ItemFarmacia_Report_Localidad @localidad='{localidad}'";
            return _context.ItemFarmacias.FromSql(query).ToArray();
        }

        public static ItemFarmacia[] ReportLocalidadInsumos(Localidad localidad)
        {
            string query = $"EXEC ItemFarmacia_Report_InsumosM @localidad='{localidad}'";
            return new MyDbContext().ItemFarmacias.FromSql(query).ToArray();
        }

        public static ItemFarmacia[] Report(Localidad localidad)
        {
            string query = $"EXEC ItemFarmacia_Report_Total @localidadActual='{localidad}'";
            return new MyDbContext().ItemFarmacias.FromSql(query).ToArray();
        }

        public static Egreso[] Egresos(Localidad localidad, DateTime desde, DateTime hasta)
        {
            string query = $"EXEC ItemReceta_Report_Items @localidad='{localidad}', @desde='{desde}', @hasta='{hasta}'";
            return new MyDbContext().Egreso.FromSql(query).ToArray();
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

        public class Data
        {
            public int item { get; set; }
            public int cantidad { get; set; }
        }

        public class Egreso
        {
            [Key]
            public int id { get; set; }
            public string nombre { get; set; }
            public string compuesto { get; set; }
            public string categoria { get; set; }
            public string grupo { get; set; }
            public int stock { get; set; }
            public DateTime? fcaducidad { get; set; }
            public string localidad { get; set; }
            public int egreso { get; set; }
        }
    }
}
