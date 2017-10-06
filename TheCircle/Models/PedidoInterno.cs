using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class PedidoInterno
    {
        [Key]
        public int id { get; set; }
        public ItemFarmacia itemFarmacia { get; set; }
        public UserSafe solicitante { get; set; }
        public int cantidad { get; set; }
        public DateTime fechaPedido { get; set; }
        public UserSafe personalDespacho { get; set; } = null;
        public int? cantidadDespacho { get; set; }
        public DateTime? fechaDespacho { get; set; }
        public string comentarioDespacho { get; set; } = null;
        public string comentarioRecepcion { get; set; } = null;
        public Boolean? cancelado { get; set; }

        public PedidoInterno() { }

        public PedidoInterno(int idItem, int cantidad, int solicitante) {
            try {
                string q = $"EXEC PedidoInterno_Insert @idItemFarmacia={idItem}, @solicitante={solicitante}, @cantidad={cantidad}";
                new MyDbContext().Database.ExecuteSqlCommand(q);

            } catch (Exception e) {
                throw new Exception("No se pudo crear el pedido interno", e);
            }            
        }

        internal static PedidoInterno[] GetInconsistentes()
        {
            string query = $"EXEC PedidoInterno_Report_Inconsistente";
            return new MyDbContext().PedidoInterno.FromSql(query).Populate();
        }

        public static PedidoInterno[] GetPendientes(Localidad localidad)
        {
            string q = $"EXEC dbo.PedidoInterno_Report_Pendientes @localidad='{localidad}'";
            return new MyDbContext().PedidoInterno.FromSql(q).Populate();
        }

        internal static PedidoInterno[] GetPendientesRecepcion(int personal)
        {
            string q = $"EXEC PedidoInterno_Report_PendientesRecepcion @solicitante={personal}";
            return new MyDbContext().PedidoInterno.FromSql(q).Populate();
        }

        internal static PedidoInterno[] GetReceptadas(Localidad destino, MyDbContext _c)
        {
            string q = $"EXEC PedidoInterno_Report_Receptadas @destino={destino}";
            return _c.PedidoInterno.FromSql(q).Populate();
        }

        public static void Despachar(int idPedido, int personal, int cantidad, string comentario)
        {
            string q = $"EXEC PedidoInterno_Despachar @id={idPedido}, @personal={personal}, @cantidad={cantidad}, @comentario='{comentario}'";
            new MyDbContext().Database.ExecuteSqlCommand(q);
        }

        public static void Recepcion(int idPedido, string comentario, int personal)
        {
            string q = $"EXEC PedidoInterno_Recepcion @id={idPedido}, @comentario='{comentario}', @solicitante={personal}";
            new MyDbContext().Database.ExecuteSqlCommand(q);
        }

        public class Data
        {
            public int cantidad { get; set; }
            public string comentario { get; set; } = null;
        }

        public class BDD
        {
            [Key]
            public int id { get; set; }
            public int idItemFarmacia { get; set; }
            public int solicitante { get; set; }
            public int cantidad { get; set; }
            public DateTime fechaPedido { get; set; }
            public int? personalDespacho { get; set; }
            public int? cantidadDespacho { get; set; }
            public DateTime? fechaDespacho { get; set; }
            public string comentarioDespacho { get; set; } = null;
            public string comentarioRecepcion { get; set; } = null;
            public Boolean? cancelado { get; set; }
        }

    }
}
