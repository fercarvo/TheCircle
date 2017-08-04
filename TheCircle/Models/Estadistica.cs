

using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TheCircle.Models
{
    public class EstadisticaEnfermedad
    {
        [Key]
        public string codigo { get; set; }
        public string nombre { get; set; }
        public int veces { get; set; }

        public EstadisticaEnfermedad() { }

        public EstadisticaEnfermedad[] getAll(string desde, string hasta, int apadrinado, MyDbContext _context)
        {
            try {
                string query = "EXEC dbo.report_EnfermedadByFechaByVeces @desde='" + desde +
                    "', @hasta='" + hasta +
                    "', @apadrinado=" + apadrinado;

                var data = _context.EstadisticaEnfermedad.FromSql(query);
                return data.ToArray();
            } catch (Exception e) {
                return null;
            }
        }

    }

    public class EstadisticaEnfermedadReq
    {
        public string desde { get; set; }
        public string hasta { get; set; }
        public int apadrinado { get; set; }

        public EstadisticaEnfermedadReq() { }
    }
}
