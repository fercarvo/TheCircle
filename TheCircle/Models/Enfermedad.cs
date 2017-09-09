using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
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

        public ReporteEnfermedad[] getAll(Fecha req, Localidad localidad, MyDbContext _context)
        {
            string query = $"EXEC Enfermedad_Report_Fecha @desde='{req.desde}', @hasta='{req.hasta}', @localidad='{localidad}'";
            return _context.ReporteEnfermedad.FromSql(query).ToArray();
        }
    }
}
