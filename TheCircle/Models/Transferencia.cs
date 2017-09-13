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

        public Transferencia() { }

        public Transferencia (int item, Localidad destino, int cantidad, int personal, MyDbContext _c) {
            try {
                string q = $"EXEC Transferencia_Insert @item={item}, @cantidad={cantidad}, @solicitante={personal}, @destino='{destino}'";
                _c.Database.ExecuteSqlCommand(q);

            } catch (Exception e) {
                throw new Exception("Error al crear transferencia", e);
            }             
        }

        public static Transferencia[] GetPendientes(Localidad localidad, MyDbContext _context)
        {
            string q = $"EXEC Transferencia_Report @pendientes=1, @cancelado=0, @localidadOrigen={localidad}";
            return _context.Transferencia.FromSql(q).ToArray();
        }

        internal static Transferencia[] GetDespachadas(Localidad destino, MyDbContext _context)
        {
            string q = $"EXEC Transferencia_Report_Despachadas @destino={destino}";
            return _context.Transferencia.FromSql(q).ToArray();
        }

        public static void Despachar(int personal, Data req, MyDbContext _context)
        {
            var q = $"EXEC Transferencia_Despachar @itemTransferencia={req.idTransferencia}, @cantidad={req.cantidad}, @personal={personal}, @comentario='{req.comentario}'";
            _context.Database.ExecuteSqlCommand(q);   
        }

        public class Data
        {
            public int idTransferencia { get; set; }
            public int cantidad { get; set; }
            public string comentario { get; set; }
        }
    }

}
