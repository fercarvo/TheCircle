using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            string query = $"EXEC ItemFarmacia_Report @localidadActual='{localidad}'";
            return new MyDbContext().ItemFarmacias.FromSql(query).ToArray();
        }

        public static ItemFarmacia[] ReportTotal()
        {
            string query = "EXEC ItemFarmacia_Report_Total";
            return new MyDbContext().ItemFarmacias.FromSql(query).ToArray();
        }

        public static Nombre[] ReportNombres() {
            string query = $"EXEC ItemFarmacia_Report_Nombre";
            return new MyDbContext().NombresItem.FromSql(query).ToArray();
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
            public string localidad { get; set; } = null;
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

        public class Nombre
        {
            [Key]
            public Int64 indice { get; set; }
            public string nombre { get; set; }
            public string compuesto { get; set; }
        }
    }

    public class ItemFarmacia2
    {
        [Key]
        public int id { get; set; }
        public string nombre { get; set; }
        public Compuesto compuesto { get; set; }
        public int stock { get; set; }
        public DateTime? fcaducidad { get; set; }
        public string localidad { get; set; }

        public ItemFarmacia2() { }

        public ItemFarmacia2(BDD BDD) {
            id = BDD.id;
            nombre = BDD.nombre;
            compuesto = Compuesto.Get(BDD.compuesto);
            stock = BDD.stock;
            fcaducidad = BDD.fcaducidad;
            localidad = BDD.localidad;
        }

        public static ItemFarmacia2[] Populate(IQueryable<BDD> data, MyDbContext _context)
        {
            var items = new List<ItemFarmacia2>();

            foreach (var item in data)
                items.Add(new ItemFarmacia2(item));

            return items.ToArray();
        }

        public static ItemFarmacia2[] ReportLocalidad(Localidad localidad, MyDbContext _context)
        {
            string query = $"EXEC ItemFarmacia_Report_Localidad @localidad='{localidad}'";
            var data = _context.ItemFarmaciaBDD.FromSql(query);

            return Populate(data, _context);
        }

        public class BDD
        {
            [Key]
            public int id { get; set; }
            public string nombre { get; set; }
            public int compuesto { get; set; }
            public int stock { get; set; }
            public DateTime? fcaducidad { get; set; }
            public string localidad { get; set; }

            public BDD() { }
        }

    }
}
