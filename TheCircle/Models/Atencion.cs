
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace TheCircle.Models
{
    public class Atencion 
    {
        public int id { get; set; }
        public string idApadrinado { get; set; }
        public string nombreApadrinado { get; set; }
        public int idDoctor { get; set; }
        public string nombreDoctor { get; set; }
        public DateTime fecha { get; set; }
        public double? peso { get; set; }
        public double? talla { get; set; }
        public string tipo { get; set; }

        public Atencion () { }

        public Atencion crear(AtencionNueva request, MyDbContext _context) 
        {
            Atencion atencion;

            if (request != null) {
                string query = "DECLARE @id int " +
                    "EXEC dbo.insert_Atencion2 @apadrinado=" + request.apadrinado+
                    ", @doctor="+ request.doctor +
                    ", @tipo=" + request.tipo +
                    ", @id = @id OUTPUT";

                try {
                    atencion = _context.Atenciones.FromSql(query).First(); //Retorna la AtencionM creada
                } catch (Exception e) {
                    return null;
                }

                if (atencion != null) {
                    Diagnostico d = new Diagnostico();

                    foreach (string diagnostico in request.diagnosticos) {
                        d.insert(diagnostico, atencion.id, _context);
                    }
                    return atencion;
                } else {
                    return null;
                }
            } else {
                return null;
            }
        }

    }

    public class AtencionNueva
    {
        public int doctor { get; set; }
        public int apadrinado { get; set; }
        public string tipo { get; set; }
        public string[] diagnosticos { get; set; }

        public AtencionNueva() { }
    }

    public class AtencionResponse
    {
        public Atencion atencion { get; set; }
        public Diagnostico[] diagnosticos { get; set; }

        public AtencionResponse() { }
    }


}
