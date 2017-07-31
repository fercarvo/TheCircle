
using System;
using System.Collections.Generic;

namespace TheCircle.Models
{
    public class Atencion :
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

        public Atencion (AtencionNueva a, MyDbContext _context) {
            if (a) {
                string query = "DECLARE @id int " +
                    "EXEC dbo.insert_AtencionM @apadrinado=" + a.apadrinado+
                    ", @doctor="+ a.doctor +
                    ", @tipo=" + a.tipo +
                    ", @id = @id OUTPUT";
                try {
                    var atencionesDB = _context.Atenciones.FromSql(query); //Retorna la AtencionM creada
                    this = atencionesDB.First(); //Atencion creada

                } catch (Exception e) {
                    this = null;
                }

                if (this) {
                    foreach (string diagnostico in a.diagnosticos) {
                        Diagnostico.insert(diagnostico, this.id, _context);
                    }
                }

            } else {
              this = null;
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

    public class AtencionDiagnostico
    {
        public Atencion atencion { get; set; }
        public List<Diagnostico> diagnosticos { get; set; }

        public AtencionDiagnostico() { }
    }

    public class AtencionResponse
    {
        public Atencion atencion { get; set; }
        public Diagnostico[] diagnosticos { get; set; }

        public AtencionResponse() { }
    }


}
