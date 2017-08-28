using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

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
        public decimal monto { get; set; }
        public string sintomas { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fCaducidad { get; set; }

        public Remision() { }

        public Remision crear(RemisionRequest request, MyDbContext _context)
        {
            string query = $"DECLARE @id int EXEC dbo.insert_Remision @atencionM={request.atencionM}" +
              $", @institucion={request.institucion}" +
              $", @monto='{request.monto}'" +
              $", @sintomas='{request.sintomas}', @id=@id OUTPUT";

            try {
                var data = _context.Remisiones.FromSql(query).First();
                return data;

            } catch (Exception e) {
                Console.WriteLine(e);
                throw new Exception("Error crear remision medica, Remision.crear");
            }
        }
    }

    public class RemisionRequest
    {
        public int atencionM { get; set; }
        public int institucion { get; set; }
        public int monto { get; set; }
        public string sintomas { get; set; }

        public RemisionRequest() { }
    }
}
