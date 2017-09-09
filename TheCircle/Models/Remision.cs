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
        public int atencionM { get; set; }
        public int doctor { get; set; }
        public Int32 IdInstitucion { get; set; }
        public string nombreInstitucion { get; set; }
        public decimal monto { get; set; }
        public string sintomas { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fCaducidad { get; set; }

        public Remision() { }

        public void crear(RemisionRequest request, MyDbContext _context)
        {
            string query = $"EXEC dbo.Remision_insert @atencionM={request.atencionM}, @institucion={request.institucion}, @monto='{request.monto}', @sintomas='{request.sintomas}'";
            _context.Database.ExecuteSqlCommand(query);
        }
    }

    public class RemisionRequest
    {
        public int atencionM { get; set; }
        public int institucion { get; set; }
        public int monto { get; set; }
        [StringLength(50)]
        public string sintomas { get; set; }

        public RemisionRequest() { }
    }

    public class ReporteRemision
    {
        [Key]
        public int id { get; set; }
        public int codigoApadrinado { get; set; }
        public string institucion { get; set; }
        public string especialidad { get; set; }
        public decimal monto { get; set; }
        public string sintomas { get; set; }
        public DateTime fecha { get; set; }

        public ReporteRemision() { }

        public ReporteRemision[] getAll_Doctor_Date(Fecha req, int doctor, MyDbContext _context)
        {
            string query = $"EXEC dbo.report_RemisionByDoctor @desde='{req.desde}', @hasta='{req.hasta}', @doctor={doctor}";
            try {
                var data = _context.ReporteRemision.FromSql(query).ToArray();
                return data;
            } catch (Exception e) {
                throw new Exception("Error al cargar remisiones by doctor, ReporteRemision.getAll");
            }
        }

    }
}
