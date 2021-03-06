﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        public ItemFarmacia(IngresoRequest item, Localidad localidad, int personal) {
            try {
                string query = $"DECLARE @item_id int EXEC ItemFarmacia_Insert_Docs @nombre='{item.nombre}', " +
                    $"@compuesto={item.compuesto}, " +
                    $"@fcaducidad='{item.fcaducidad}', " +
                    $"@cantidad={item.cantidad}, " +
                    $"@localidad='{localidad}', " +
                    $"@orden='{item.orden}', " +
                    $"@documento='{item.documento}', " +
                    $"@proveedor='{item.proveedor}', " +
                    $"@personal={personal}, @item_id=@item_id OUTPUT";

                new MyDbContext().Database.ExecuteSqlCommand(query);

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

        public static Proveedor[] ReportProveedor()
        {
            string query = $"EXEC ItemFarmacia_Report_Proveedor";
            return new MyDbContext().Proveedores.FromSql(query).ToArray();
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

        public static Registro[] ReportRegistro(int personal, DateTime desde, DateTime hasta)
        {
            string query = $"EXEC ItemFarmacia_Report_Registro @personal={personal}, @desde='{desde}', @hasta='{hasta}'";
            return new MyDbContext().RegistroItem.FromSql(query).ToArray();
        }

        public static Update[] ReportAlteraciones(int personal, DateTime desde, DateTime hasta) {
            string query = $"EXEC ItemFarmacia_Report_Alteraciones @personal={personal}, @desde='{desde}', @hasta='{hasta}'";
            return new MyDbContext().Alteraciones.FromSql(query).ToArray();
        }

        public class Update
        {
            [Key]
            public int id { get; set; }
            public string nombre { get; set; }
            public string compuesto { get; set; }
            public string categoria { get; set; }
            public string grupo { get; set; }
            public int cantidad { get; set; }
            public string localidad { get; set; }
            public DateTime fechaRegistro { get; set; }
            public DateTime? fcaducidad { get; set; }
            public int cedulaPersonal { get; set; }
            public int antiguaCantidad { get; set; }
            public int? antiguoItem { get; set; }
            public string comentario { get; set; } = null;
        }

        public class Registro
        {
            [Key]
            public int id { get; set; }
            public string nombre { get; set; }
            public string compuesto { get; set; }
            public string categoria { get; set; }
            public string grupo { get; set; }
            public int cantidad { get; set; }
            public string localidad { get; set; }
            public DateTime fechaRegistro { get; set; }
            public DateTime? fcaducidad { get; set; }
            public int cedulaPersonal { get; set; }
            public string nombrePersonal { get; set; }
            public int? transferencia { get; set; }
            public string emailPersonal { get; set; } = null;
            public string proveedor { get; set; } = null;
            public string codigoOrden { get; set; } = null;
            public string codigoDocumento { get; set; } = null;
            public string comentario { get; set; } = null;
        }

        public class IngresoTransferencia
        {
            public int idTransferencia { get; set; }
            public string comentario { get; set; }

            public IngresoTransferencia() { }
        }

        public class IngresoRequest
        {
            public string nombre { get; set; } //Ya no es requerido, recibe ""
            [Required]
            public int compuesto { get; set; } //ID del compuesto
            [Required]
            public string fcaducidad { get; set; }
            [Required]
            public int cantidad { get; set; }
            [Required]
            public string orden { get; set; } //Codigo de orden de compra
            [Required]
            public string proveedor { get; set; } //Nombre del proveedor
            public string documento { get; set; }  = null; //Codigo de numero de documento

            public IngresoRequest() { }
        }

        public static void Editar(int idItem, int personal, int nuevaCantidad, string comentario)
        {
            string query = $"EXEC ItemFarmacia_Alterar_Stock @idItem={idItem}, @nuevaCantidad={nuevaCantidad}, @personal={personal}, @comentario='{comentario}'";
            new MyDbContext().Database.ExecuteSqlCommand(query);
        }

        internal static ItemFarmacia Get(int id)
        {
            return new MyDbContext().ItemFarmacias.FromSql($"EXEC ItemFarmacia_Select @id={id}").First();
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
            //public Int64 indice { get; set; }
            public string nombre { get; set; }
            //public int? compuesto { get; set; }
        }

        public class Proveedor
        {
            [Key]
            public string nombre { get; set; }
        }
    }
}
