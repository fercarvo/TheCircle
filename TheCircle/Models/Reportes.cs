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
        public string idApadrinado { get; set; }
        public string nombreApadrinado { get; set; }
        public DateTime fecha { get; set; }
        public string tipo { get; set; }

        public ReporteAtencion() { }

        public ReporteAtencion[] getAll(ReporteRequest req, MyDbContext _context)
        {
            //try {
                string query = $"EXEC dbo.report_AtencionByDateByDoctor @desde='{req.desde}', @hasta='{req.hasta}', @doctor={req.doctor}";
                return _context.ReporteAtencion.FromSql(query).ToArray();
            //} catch (Exception e) {
                //return null;
            //}
        }

    }

    public class ReporteRemision
    {
        [Key]
        public int codigoApadrinado { get; set; }
        public string institucion { get; set; }
        public decimal monto { get; set; }
        public string sintomas { get; set; }
        public DateTime fecha { get; set; }

        public ReporteRemision() { }

        public ReporteRemision[] getAll(ReporteRequest req, MyDbContext _context)
        {
            //try {
                string query = $"EXEC dbo.report_RemisionByDateByDoctor @desde='{req.desde}', @hasta='{req.hasta}', @doctor={req.doctor}";
                return _context.ReporteRemision.FromSql(query).ToArray();
            //} catch (Exception e) {
                //return null;
            //}
        }

    }

    public class ReporteReceta
    {
        [Key]
        public int idReceta { get; set; }
        public DateTime fechaReceta { get; set; }
        public int? despachada { get; set; }

        public string enfermedadCod { get; set; }
        public string enfermedadNombre { get; set; }

        public string nombreItemFarmacia { get; set; }

        public int idApadrinado { get; set; }
        public int nombreApadrinado { get; set; }

        public ReporteReceta() { }

        public ReporteReceta[] getAll(ReporteRequest req, MyDbContext _context)
        {
            //try {
                string query = $"EXEC dbo.report_RecetaByDateByDoctor @desde='{req.desde}', @hasta='{req.hasta}', @doctor={req.doctor}";
                return _context.ReporteReceta.FromSql(query).ToArray();
            //} catch (Exception e) {
                //return null;
            //}
        }

    }
}
