using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TheCircle.Models
{
    public class Receta
    {
        [Key]
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fCaducidad { get; set; }
        public int? despachada { get; set; }
        public int? eliminada { get; set; }
        public int idDoctor { get; set; }
        public int idApadrinado { get; set; }

        public Receta () { }

        public Receta crear (RecetaRequest request, MyDbContext _context) {

            Receta receta;
            string query = $"DECLARE @id int EXEC dbo.insert_Receta @idDoctor={request.doctor}" +
              $", @idApadrinado={request.apadrinado}, @id = @id OUTPUT";

            try {
                receta = _context.Recetas.FromSql(query).First();
                return receta;
            } catch (Exception e) {
                return null;
            }
        }
    }

    public class RecetaRequest
    {
        public int doctor { get; set; }
        public int apadrinado { get; set; }

        public RecetaRequest() { }
    }
}
