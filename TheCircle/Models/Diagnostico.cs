
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class Diagnostico
    {
        public int id { get; set; }
        public string enfermedadCod { get; set; }
        public string enfermedadNombre { get; set; }

        public Diagnostico() { }

        public Diagnostico (string enfermedadCod, int atencion, MyDbContext _context)
        {
            string q = $"EXEC dbo.insert_Diagnostico @enfermedad='{enfermedadCod}', @atencion={atencion}";

            try {
                _context.Database.ExecuteSqlCommand(q); //Se inserta en la BD el diagnostico
            } catch (Exception e) {
            }  
        }

        /*
        public void insert (string enfermedadCod, int atencion, MyDbContext _context)
        {
            string q = $"EXEC dbo.insert_Diagnostico @enfermedad='{enfermedadCod}', @atencion={atencion}";

            try {
                _context.Database.ExecuteSqlCommand(q); //Se inserta en la BD el diagnostico
            } catch (Exception e) {
            }
        }*/

        public static Diagnostico[] ReportByAtencion (int idAtencion, MyDbContext _context)
        {
            string q = $"EXEC dbo.select_DiagnosticoByAtencion @atencion={idAtencion}";

            try {
                var data = _context.Diagnosticos.FromSql(q).ToArray(); //Retorna los diagnosticos de esa AtencionM
                return data;
            } catch (Exception e) {
                return null;
            }
        }

    }
}
