using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class PedidoInterno
    {
        [Key]
        public int id { get; set; }
        public string nombre { get; set; }
        public string compuesto { get; set; }
        public int solicitante { get; set; }
        public int cantidad { get; set; }
        public DateTime fechaPedido { get; set; }
        public int? personalDespacho { get; set; }
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
            return new MyDbContext().PedidoInterno.FromSql(query).ToArray();
        }

        public static PedidoInterno[] GetPendientes(Localidad localidad, MyDbContext _c)
        {
            string q = $"EXEC dbo.PedidoInterno_Report_Pendientes @localidad='{localidad}'";

            var data = _c.PedidoInterno.FromSql(q).ToArray();
            return data;
        }

        internal static PedidoInterno[] GetDespachadas(Localidad destino, MyDbContext _c)
        {
            string q = $"EXEC PedidoInterno_Report_Despachadas @destino={destino}";

            return _c.PedidoInterno.FromSql(q).ToArray();
        }

        internal static PedidoInterno[] GetReceptadas(Localidad destino, MyDbContext _c)
        {
            string q = $"EXEC PedidoInterno_Report_Receptadas @destino={destino}";

            return _c.PedidoInterno.FromSql(q).ToArray();
        }

        public static void Despachar(int idPedido, int personal, int cantidad, string comentario)
        {
            string q = $"EXEC PedidoInterno_Despachar @id={idPedido}, @personal={personal}, @cantidad={cantidad}, @comentario='{comentario}'";
            new MyDbContext().Database.ExecuteSqlCommand(q);
        }

        public static void Recepcion(int idPedido, string comentario, int personal, MyDbContext _c)
        {
            string q = $"EXEC PedidoInterno_Recepcion @id={idPedido}, @comentario='{comentario}', @solicitante={personal}";
            _c.Database.ExecuteSqlCommand(q);
        }

        public class Data
        {
            public int cantidad { get; set; }
            public string comentario { get; set; } = null;
        }

    }
}
