using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TheCircle.Util;


namespace TheCircle.Models
{
    public class Enfermedad
    {
        [Key]
        public string codigo { get; set; }
        public string nombre { get; set; }

        public Enfermedad() { }
    }

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
}
