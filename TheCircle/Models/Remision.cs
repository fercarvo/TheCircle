using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        public Int32 monto { get; set; }
        public string sintomas { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fCaducidad { get; set; }

        public Remision() { }

        public Remision crear(RemisionNueva request, MyDbContext _context) {

            Remision remision;
            try {
                string query = "DECLARE @id int" +
                  " EXEC dbo.insert_Remision @atencionM=" + request.atencionM +
                  ", @institucion=" + request.institucion +
                  ", @monto=" + request.monto +
                  ", @sintomas=" + request.sintomas +
                  ", @id = @id OUTPUT";

                remision = _context.Remisiones.FromSql(query).First();
                //remision = data.First(); //Remision creada
                return remision;
            } catch (Exception e) {
                return null;
            }
        }
    }

    public class RemisionNueva
    {
        public int atencionM { get; set; }
        public int institucion { get; set; }
        public int monto { get; set; }
        public string sintomas { get; set; }

        public RemisionNueva() { }
    }
}
