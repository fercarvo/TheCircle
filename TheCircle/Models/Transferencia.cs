using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class Transferencia
    {
        [Key]
        public int id { get; set; }
        public string nombre { get; set; }
        public string compuesto { get; set; }
        public int solicitante { get; set; }
        public int cantidad { get; set; }
        public DateTime fecha { get; set; }
        public string origen { get; set; }
        public string destino { get; set; }
        public Boolean cancelado { get; set; }
        public int? autorizadoPor { get; set; }
        public DateTime? fechaDespacho { get; set; }
        public int? cantidadDespacho { get; set; }
        public int? personalDespacho { get; set; }
        public string comentarioDespacho { get; set; } = null;

        public Transferencia (int item, Localidad destino, int cantidad, int personal, MyDbContext _c) {
            try {
                string q = $"EXEC Transferencia_Insert @item={item}, @cantidad={cantidad}, @personal={personal}, @localidad='{localidad}'";
                _c.Database.ExecuteSqlCommand(q);

            } catch (Exception e) {
                throw new Exception("Error al crear transferencia");
            }             
        }

        public static Transferencia[] GetPendientes(Localidad localidad, MyDbContext _context)
        {
            string q = $"EXEC ItemTransferencia_Report @pendientes=1, @cancelado=0, @localidadOrigen={localidad}";
            //string q = $"EXEC Transferencia_Report @pendientes=1, @cancelado=0, @localidadOrigen={localidad}";

            var data = _context.Transferencia.FromSql(q).ToArray();
            return data;
        }

        internal static Transferencia[] GetDespachadas(Localidad destino, MyDbContext _context)
        {
            string q = $"EXEC ItemTransferencia_Report_Despachadas @destino={destino}";
            //string q = $"EXEC Transferencia_Report_Despachadas @destino={destino}";

            var data = _context.Transferencia.FromSql(q).ToArray();
            return data;
        }

        public static void Despachar(int personal, TransferenciaRequest req, MyDbContext _context) 
        {
            string q = $"EXEC ItemTransferencia_Despachar @itemTransferencia={req.idTransferencia}, @cantidad={req.cantidad}, @personal={personal}, @comentario='{req.comentario}'";
            //string q = $"EXEC Transferencia_Despachar @itemTransferencia={req.idTransferencia}, @cantidad={req.cantidad}, @personal={personal}, @comentario='{req.comentario}'";

            _context.Database.ExecuteSqlCommand(q);   
        }
    }

    public class TransferenciaRequest
    {
        public int idTransferencia { get; set; }
        public int cantidad { get; set; }
        public string comentario { get; set; }
    }
}
