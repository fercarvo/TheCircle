using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        public DateTime fecha { get; set; }
        public DateTime fCaducidad { get; set; }

        public Remision() { }

        public Remision(RemisionNueva remision, MyDbContext _context) {
            try {
                string query = "DECLARE @id int" +
                  " EXEC dbo.insert_Remision @atencionM=" + remision.atencionM +
                  ", @institucion=" + remision.institucion +
                  ", @monto=" + remision.monto +
                  ", @sintomas=" + remision.sintomas +
                  ", @id = @id OUTPUT";

                var data = _context.Apadrinados.FromSql(query);
                this = data.First(); //Remision creada
            } catch (Exception e) {
                this = null;
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
