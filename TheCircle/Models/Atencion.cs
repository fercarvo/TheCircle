
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class Atencion
    {
        public int id { get; set; }
        public Int32 idApadrinado { get; set; }
        public string nombreApadrinado { get; set; }
        public string apellidoApadrinado { get; set; }
        public int idDoctor { get; set; }
        public DateTime fecha { get; set; }
        public double? peso { get; set; }
        public double? talla { get; set; }
        public string tipo { get; set; }
        public string localidad { get; set; }

        public Atencion () { }

        public Atencion crear(AtencionRequest request, int doctor, Localidad localidad, MyDbContext _context)
        {
            Atencion atencion;
            Diagnostico d = new Diagnostico();

            string query = $"DECLARE @id int EXEC dbo.insert_Atencion @apadrinado={request.apadrinado}" +
                $", @doctor={doctor}" +
                $", @tipo={request.tipo}" +
                $", @localidad={localidad}" +
                $", @peso='{request.peso}'" +
                $", @talla='{request.talla}'" +
                $", @id = @id OUTPUT";

            try {
                atencion = _context.Atenciones.FromSql(query).First(); //atencion medica creada
                foreach (string diagnostico in request.diagnosticos) { //Se ingresan los diagnosticos en la atencion
                    d.insert(diagnostico, atencion.id, _context);
                }
                return atencion;

            } catch (Exception e) {
                throw new Exception("Error crear/cargar atencion medica, Atencion.crear");
            }
        }

        public Atencion[] getBy_doctor_date(Fecha req, int doctor, MyDbContext _context)
        {
            string query = $"EXEC dbo.report_Atencion_Doctor @desde='{req.desde}', @hasta='{req.hasta}', @doctor={doctor}";
            try {
                var data = _context.Atenciones.FromSql(query).ToArray();
                return data;
            } catch (Exception e) {
                throw new Exception("Error cargar atenciones by doctor, Atencion.getBy_doctor");
            }
        }

    }

    public class AtencionRequest
    {
        public int apadrinado { get; set; }
        public string tipo { get; set; }
        public string[] diagnosticos { get; set; }
        public int? peso { get; set; }
        public int? talla { get; set; }

        public AtencionRequest() { }
    }

    
    public class AtencionResponse
    {
        public Atencion atencion { get; set; }
        public Diagnostico[] diagnosticos { get; set; }

        public AtencionResponse() { }
    }
    
}
