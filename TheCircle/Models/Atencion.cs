
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public Atencion (Data request, int doctor, Localidad localidad, MyDbContext _context) {
            try {
                string q = $"EXEC dbo.Atencion_Insert @apadrinado={request.apadrinado}" +
                    $", @doctor={doctor}" +
                    $", @tipo={request.tipo}" +
                    $", @localidad={localidad}" +
                    $", @peso='{request.peso}'" +
                    $", @talla='{request.talla}'";

                var data = _context.Atenciones.FromSql(q).First();
                
                foreach (var dg in request.diagnosticos) //Se crean los diagnosticos de la atencion
                    new Diagnostico(dg, data.id, _context);

                this.id = data.id;

            } catch (Exception e) {
                throw new Exception("No se pudo crear la atencion medica");
            }
        }

        public static Atencion[] ReportByDoctorDate(Fecha req, int doctor, MyDbContext _context)
        {
            string query = $"EXEC Atencion_Report_Doctor @desde='{req.desde}', @hasta='{req.hasta}', @doctor={doctor}";
            var data = _context.Atenciones.FromSql(query).ToArray();

            return data;
        }

        public static Stadistics[] Report(DateTime desde, DateTime hasta)
        {
            string query = $"EXEC Atencion_Report_Total @desde='{desde}', @hasta='{hasta}'";
            return new MyDbContext().Stadistics.FromSql(query).ToArray();
        }

        public static void AlertaPesoTalla(Data request, Token token) {
            try {
                Apadrinado apadrinado = Apadrinado.Get(request.apadrinado, new MyDbContext());
                UserSafe operador = UserSafe.GetByCargo("operador");

                var peso = (float)request.peso / (float)apadrinado.peso;
                var talla = (float)request.talla / (float)apadrinado.talla;

                if (peso < 0.8 || peso > 1.2)
                    new EmailTC().AlertaPesoTalla(operador.nombre, operador.email, request.apadrinado, request.peso, request.talla, $"{token.data.nombres} {token.data.apellidos}");
                else if (talla < 0.8 || talla > 1.2)
                    new EmailTC().AlertaPesoTalla(operador.nombre, operador.email, request.apadrinado, request.peso, request.talla, $"{token.data.nombres} {token.data.apellidos}");
                
            } catch (Exception e) {
            }           
        }

        public class Data
        {
            public int apadrinado { get; set; }
            public string tipo { get; set; }
            public string[] diagnosticos { get; set; }
            public int? peso { get; set; }
            public int? talla { get; set; }
        }

        public class Stadistics
        {
            [Key]
            public string localidad { get; set; }
            public int cantidad { get; set; }
        }

    }    
}
