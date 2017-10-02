using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class Remision
    {
        [Key]
        public int id { get; set; }
        public int codigoApadrinado { get; set; }
        public string institucion { get; set; }
        public string especialidad { get; set; }
        public decimal monto { get; set; }
        public string sintomas { get; set; }
        public DateTime fecha { get; set; }

        public int cedulaDoctor { get; set; }
        public string nombreDoctor { get; set; }
        public string apellidoDoctor { get; set; }
        public string email { get; set; }
        public string nombreApadrinado { get; set; }
        public DateTime fCaducidad { get; set; }

        public Remision() { }

        public Remision (int atencionM, int institucion, int monto, string sintomas)
        {
            try {
                string q = $"EXEC Remision_insert @atencionM={atencionM}, @institucion={institucion}, @monto='{monto}', @sintomas='{sintomas}'";
                new MyDbContext().Database.ExecuteSqlCommand(q);

            } catch (Exception e) {
                throw new Exception("Error al crear remision medica", e);
            }            
        }

        public static Remision[] GetPendientes()
        {
            var data = new MyDbContext().Remision.FromSql("EXEC Remision_Report").ToArray();
            return data;
        }

        public static Remision[] ReportByDoctorDate(Fecha req, int doctor)
        {
            string query = $"EXEC Remision_Report_DoctorDate @desde='{req.desde}', @hasta='{req.hasta}', @doctor={doctor}";

            var data = new MyDbContext().Remision.FromSql(query).ToArray();
            return data;
        }

        public static Aprobacion[] ReportAprobacion1()
        {
            string query = "EXEC Remision_Report_Aprobacion1";
            return new MyDbContext().Aprobacion.FromSql(query).ToArray();
        }
    

        public class Request
        {
            public int atencionM { get; set; }
            public int institucion { get; set; }
            public int monto { get; set; }
            [StringLength(50)]
            public string sintomas { get; set; }
        }

        public static Aprobacion[] GetAP1Rechazadas()
        {
            string query = "EXEC Remision_Report_Rechazadas";
            return new MyDbContext().Aprobacion.FromSql(query).ToArray();
        }

        public static void RechazarAP1(int id, string comentario)
        {
            string query = $"EXEC Remision_Aprobacion1_Rechazar @remision={id}, @comentarioRechazo='{comentario}'";
            new MyDbContext().Database.ExecuteSqlCommand(query);
        }

        public static void ReAprobarAP1(int cedula, int id, string comentario, double monto)
        {
            string query = $"EXEC Remision_Aprobacion1_Update1 @comentario='{comentario}', @monto='{monto}', @remision={id}, @personal={cedula}";
            new MyDbContext().Database.ExecuteSqlCommand(query);
        }

        public class Aprobacion {
            [Key]
            public int idRemision { get; set; }
            public Decimal monto { get; set; }
            public DateTime fecha { get; set; }
            public DateTime fCaducidad { get; set; }
            public string sintomas { get; set; }
            public string institucion { get; set; }
            public Double montoAP1 { get; set; }
            public DateTime fechaAP1 { get; set; }
            public string comentarioAP1 { get; set; } = null;
            public int personalAP1 { get; set; }
            public string comentarioRechazo { get; set; } = null;
            public int? personalContralor { get; set; } //null
            public DateTime? fechaContralor { get; set; } //null
            public string comentarioContralor { get; set; } = null;

            public Aprobacion() { }

            public Aprobacion(int remision, Double monto, string comentario, int personal) {
                try {
                    string q = $"EXEC Remision_Aprobacion1_Insert @idRemision={remision}, @monto='{monto}', @comentario='{comentario}', @personal='{personal}'";
                    new MyDbContext().Database.ExecuteSqlCommand(q);

                } catch (Exception e) {
                    throw new Exception("Error al crear Aprobacion 1", e);
                }
            }
        }

    }
}
