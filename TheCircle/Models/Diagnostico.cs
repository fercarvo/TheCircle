
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace TheCircle.Models
{
    public class Diagnostico
    {
        public int id { get; set; }
        public string enfermedadCod { get; set; }
        public string enfermedadNombre { get; set; }

        public Diagnostico() { }

        public void insert (string enfermedadCod, int atencion, MyDbContext _context)
        {
            try {
                string q = "EXEC dbo.insert_Diagnostico @enfermedad=" + enfermedadCod + ", @atencion=" + atencion;
                _context.Database.ExecuteSqlCommand(q); //Se inserta en la BD el diagnostico
            } catch (Exception e) {
            }
        }

        public Diagnostico[] getAllByAtencion (int idAtencion, MyDbContext _context)
        {
            string q = "EXEC dbo.select_DiagnosticoByAtencion @atencion=" + idAtencion;

            try {
                var data = _context.Diagnosticos.FromSql(q); //Retorna los diagnosticos de esa AtencionM
                return data.ToArray();
            } catch (Exception e) {
                return null;
            }
        }

    }

}
