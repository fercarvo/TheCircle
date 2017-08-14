using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TheCircle.Models
{
    public class ReporteEnfermedad
    {
        [Key]
        public string codigo { get; set; }
        public string nombre { get; set; }
        public int veces { get; set; }

        public ReporteEnfermedad() { }

        public ReporteEnfermedad[] getAll(ReporteRequest req, MyDbContext _context)
        {
            try {
                string query = $"EXEC dbo.report_EnfermedadByFecha @desde='{req.desde}', @hasta='{req.hasta}', @localidad={req.localidad}";
                return _context.ReporteEnfermedad.FromSql(query).ToArray();
            } catch (Exception e) {
                return null;
            }
        }

    }

    public class ReporteRequest
    {
        public string desde { get; set; }
        public string hasta { get; set; }
        public string localidad { get; set; }
        public int doctor { get; set;}

        public ReporteRequest() { }
    }

    public class ReporteAtencion
    {
        [Key]
        public int id { get; set; }
        public Int32 idApadrinado { get; set; }
        public DateTime fecha { get; set; }
        public string tipo { get; set; }

        public ReporteAtencion() { }

        public ReporteAtencion[] getAll(ReporteRequest req, MyDbContext _context)
        {
            try {
                string query = $"EXEC dbo.report_AtencionByDoctor @desde='{req.desde}', @hasta='{req.hasta}', @doctor={req.doctor}";
                return _context.ReporteAtencion.FromSql(query).ToArray();
            } catch (Exception e) {
                return null;
            }
        }

    }

    public class ReporteRemision
    {
        [Key]
        public int id { get; set; }
        public int codigoApadrinado { get; set; }
        public string institucion { get; set; }
        public decimal monto { get; set; }
        public string sintomas { get; set; }
        public DateTime fecha { get; set; }

        public ReporteRemision() { }

        public ReporteRemision[] getAll(ReporteRequest req, MyDbContext _context)
        {
            try {
                string query = $"EXEC dbo.report_RemisionByDoctor @desde='{req.desde}', @hasta='{req.hasta}', @doctor={req.doctor}";
               
                return _context.ReporteRemision.FromSql(query).ToArray();
            } catch (Exception e) {
                return null;
            }
        }

    }
}
