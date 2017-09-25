using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class Remision2
    {
        [Key]
        public int id { get; set; }
        public int cedulaDoctor { get; set; }
        public string nombreDoctor { get; set; }
        public string apellidoDoctor { get; set; }
        public int codigoApadrinado { get; set; }
        public int nombreApadrinado { get; set; }
        public string institucion { get; set; }
        public string especialidad { get; set; }
        public decimal monto { get; set; }        
        public DateTime fecha { get; set; }
        public DateTime fCaducidad { get; set; }
        public string sintomas { get; set; }

        public Remision2() { }

    }

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

        /*public static AP1[] GetAprobadasAP1
        {
            var data = new MyDbContext().Remision.FromSql("EXEC Remision_Report").ToArray();
            return data;
        }*/

        public class Request
        {
            public int atencionM { get; set; }
            public int institucion { get; set; }
            public int monto { get; set; }
            [StringLength(50)]
            public string sintomas { get; set; }
        }

        public class AP1 {
            [Key]
            public int remision { get; set; }
            public decimal monto { get; set; }
            public DateTime fecha { get; set; }
            public string comentario { get; set; }
            public int cedulaPersonal { get; set; }
            public string nombrePersonal { get; set; }
            public string emailPersonal { get; set; }
            public Boolean rechazado { get; set; }
        }

        public class APContralor
        {
            [Key]
            public int remision { get; set; }
            public DateTime fecha { get; set; }
            public string comentario { get; set; }
            public int cedulaPersonal { get; set; }
            public string nombrePersonal { get; set; }
            public string emailPersonal { get; set; }
        }

    }
}
