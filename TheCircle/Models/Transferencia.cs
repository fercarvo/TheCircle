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

        public Transferencia (int item, Localidad destino, int cantidad, int personal) {
            try {
                string q = $"EXEC Transferencia_Insert @item={item}, @cantidad={cantidad}, @solicitante={personal}, @destino='{destino}'";
                new MyDbContext().Database.ExecuteSqlCommand(q);

            } catch (Exception e) {
                throw new Exception("Error al crear transferencia", e);
            }             
        }

        public static Transferencia[] GetPendientes(Localidad localidad)
        {
            string q = $"EXEC Transferencia_Report @pendientes=1, @cancelado=0, @localidadOrigen={localidad}";
            return new MyDbContext().Transferencia.FromSql(q).ToArray();
        }

        internal static Transferencia[] GetDespachadas(Localidad destino, MyDbContext _context)
        {
            string q = $"EXEC Transferencia_Report_Despachadas @destino={destino}";
            return _context.Transferencia.FromSql(q).ToArray();
        }

        //Obtengo todas las transferencias inconsistentes
        internal static Transferencia[] GetInconsistentes()
        {
            string q = "EXEC Transferencia_Report_Inconsistente";
            return new MyDbContext().Transferencia.FromSql(q).ToArray();
        }

        public static void Despachar(int personal, Data req)
        {
            var q = $"EXEC Transferencia_Despachar @itemTransferencia={req.idTransferencia}, @cantidad={req.cantidad}, @personal={personal}, @comentario='{req.comentario}'";
            new MyDbContext().Database.ExecuteSqlCommand(q);   
        }

        public class Data
        {
            public int idTransferencia { get; set; }
            public int cantidad { get; set; }
            public string comentario { get; set; }
        }

        public class Final
        {
            [Key]
            public int id { get; set; }
            public string nombre { get; set; }
            public string compuesto { get; set; }
            public UserSafe solicitante { get; set; }
            public int cantidad { get; set; }
            public DateTime fecha { get; set; }
            public string origen { get; set; }
            public string destino { get; set; }
            public Boolean cancelado { get; set; }
            public UserSafe autorizadoPor { get; set; } = null;
            public DateTime? fechaDespacho { get; set; }
            public int? cantidadDespacho { get; set; }
            public UserSafe personalDespacho { get; set; } = null;
            public string comentarioDespacho { get; set; } = null;

            public Final(BDD BDD) {

                UserSafe[] usuarios = UserSafe.GetAll();

                id = BDD.id;
                nombre = BDD.nombre;
                compuesto = BDD.compuesto;
                solicitante = new UserSafe(usuarios, BDD.solicitante);
                cantidad = BDD.cantidad;
                fecha = BDD.fecha;
                origen = BDD.origen;
                destino = BDD.destino;
                cancelado = BDD.cancelado;
                autorizadoPor = new UserSafe(usuarios, BDD.autorizadoPor);
                fechaDespacho = BDD.fechaDespacho;
                cantidadDespacho = BDD.cantidadDespacho;
                personalDespacho = new UserSafe(usuarios, BDD.personalDespacho);
                comentarioDespacho = BDD.comentarioDespacho;
            }
        }

        public class BDD {
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
        }
    }

}
