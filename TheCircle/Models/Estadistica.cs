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

        public ReporteEnfermedad[] getAll(ReporteEnfermedadRequest req, MyDbContext _context)
        {
            try {
                string query = $"EXEC dbo.report_EnfermedadByFecha @desde='{req.desde}', @hasta='{req.hasta}', @localidad={req.localidad}";
                return _context.ReporteEnfermedad.FromSql(query).ToArray();
            } catch (Exception e) {
                return null;
            }
        }

    }

    public class ReporteEnfermedadRequest
    {
        public string desde { get; set; }
        public string hasta { get; set; }
        public string localidad { get; set; }

        public ReporteEnfermedadRequest() { }
    }

    public class ReporteAtencion
    {
        [Key]
        public string codigo { get; set; }
        public string nombre { get; set; }
        public int veces { get; set; }

        public ReporteAtencion() { }

        public ReporteAtencion[] getAll(ReporteAtencionRequest req, MyDbContext _context)
        {
            //try {
                string query = $"EXEC dbo.report_AtencionByDateByDoctor @desde='{req.desde}', @hasta='{req.hasta}', @doctor={req.doctor}";
                return _context.ReporteAtencion.FromSql(query).ToArray();
            //} catch (Exception e) {
                //return null;
            //}
        }

    }

    public class ReporteAtencionRequest
    {
        public string desde { get; set; }
        public string hasta { get; set; }
        public string doctor { get; set; }

        public ReporteAtencionRequest() { }
    }
}
