using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class ReporteEnfermedad
    {
        [Key]
        public string codigo { get; set; }
        public string nombre { get; set; }
        public int veces { get; set; }

        public ReporteEnfermedad() { }

        public ReporteEnfermedad[] getAll(ReporteRequest req, Localidad localidad, MyDbContext _context)
        {
            try {
                string query = $"EXEC dbo.report_EnfermedadByFecha @desde='{req.desde}', @hasta='{req.hasta}', @localidad='{localidad}'";
                return _context.ReporteEnfermedad.FromSql(query).ToArray();
            } catch (Exception e) {
                throw new Exception("Error al cargar reporte de enfermedades, ReporteEnfermedad.getAll");
            }
        }

    }

    public class ReporteRequest
    {
        [BindRequired]
        public string desde { get; set; }
        [BindRequired]
        public string hasta { get; set; }

        public ReporteRequest() { }
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

        public ReporteRemision[] getAll_Doctor_Date(ReporteRequest req, int doctor, MyDbContext _context)
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
