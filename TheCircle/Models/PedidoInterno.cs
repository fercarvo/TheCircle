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

        public PedidoInterno(int idItem, int cantidad, int solicitante, MyDbContext _c) {
            try
            {
                string q = $"EXEC PedidoInterno_Insert @idItemFarmacia={idItem}, @solicitante={solicitante}, @cantidad={cantidad}";
                _c.Database.ExecuteSqlCommand(q);
            } catch (Exception e) {
                throw new Exception("No se pudo crear el pedido interno", e);
            }            
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

        public static void Despachar(int idPedido, Data req, MyDbContext _c)
        {
            string q = $"EXEC PedidoInterno_Despachar @id={idPedido} @personal={req.personal}, @cantidad={req.cantidad}, @comentario='{req.comentario}'";
            _c.Database.ExecuteSqlCommand(q);
        }

        public static void Recepcion(int idPedido, string comentario, int personal, MyDbContext _c)
        {
            string q = $"EXEC PedidoInterno_Recepcion @id={idPedido}, @comentario='{comentario}', @solicitante={personal}";
            _c.Database.ExecuteSqlCommand(q);
        }

        public class Data
        {
            public int personal { get; set; }
            public int cantidad { get; set; }
            public string comentario { get; set; } = null;
        }

    }
}
