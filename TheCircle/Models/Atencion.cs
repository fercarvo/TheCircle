
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
        public string localidad { get; set; }

        public Atencion () { }

        public Atencion crear(AtencionRequest request, MyDbContext _context)
        {
            Atencion atencion;
            Diagnostico d = new Diagnostico();

            if (request != null) {
                string query = "DECLARE @id int " +
                    "EXEC dbo.insert_Atencion @apadrinado=" + request.apadrinado+
                    ", @doctor="+ request.doctor +
                    ", @tipo=" + request.tipo +
                    ", @localidad=" + request.localidad +
                    ", @id = @id OUTPUT";

                try {
                    atencion = _context.Atenciones.FromSql(query).First(); //Retorna la AtencionM creada
                } catch (Exception e) {
                    return null;
                }

                if (atencion != null) {
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

    public class AtencionRequest
    {
        public int doctor { get; set; }
        public int apadrinado { get; set; }
        public string tipo { get; set; }
        public string[] diagnosticos { get; set; }
        public string localidad { get; set; }

        public AtencionRequest() { }
    }

    public class AtencionResponse
    {
        public Atencion atencion { get; set; }
        public Diagnostico[] diagnosticos { get; set; }

        public AtencionResponse() { }
    }


}
