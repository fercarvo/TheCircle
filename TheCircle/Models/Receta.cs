using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


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

        public Receta (RecetaNueva receta, MyDbContext _context) {
            string query = "DECLARE @id int" +
              " EXEC dbo.insert_Receta @idDoctor=" + receta.idDoctor +
              ", @idApadrinado=" + receta.idApadrinado +
              ", @id = @id OUTPUT";

            try {
                var data = _context.Recetas.FromSql(query); //manejar errores para que no se caiga
                this = data.First();
            } catch (Exception e) {
                throw e;
            }
        }
    }

    public class RecetaNueva
    {
        public int idDoctor { get; set; }
        public int idApadrinado { get; set; }

        public RecetaNueva() { }
    }
}
