using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class Receta
    {
        [Key]
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fCaducidad { get; set; }
        public DateTime? fDespacho { get; set; }
        public Boolean? despachada { get; set; }
        public Boolean? eliminada { get; set; }
        public int idDoctor { get; set; }
        public string nombreDoctor { get; set; }
        public string apellidoDoctor { get; set; }
        public int idApadrinado { get; set; }
        public string nombreApadrinado { get; set; }
        public string apellidoApadrinado { get; set; }

        public Receta () { }

        public Receta (int apadrinado, int doctor, MyDbContext _context) {
            try {
                var q = $"EXEC Receta_Insert @doctor={doctor}, @apadrinado={apadrinado}";
                var data = _context.Recetas.FromSql(q).First();
                id = data.id;
            } catch (Exception e) {
                throw new Exception("No se pudo crear la receta con su ID", e);
            }            
        }

        /*public static Receta[] ReportLocalidad( Localidad localidad, MyDbContext _context)
        {
            string query = $"EXEC Receta_Report_Localidad @localidad='{localidad}'";
            return _context.Recetas.FromSql(query).ToArray();
        }*/


        public static Receta[] ReportAsistente(int asistente, MyDbContext _context)
        {
            string query = $"EXEC Receta_ReportBy_Asistente @asistente={asistente}";
            return _context.Recetas.FromSql(query).ToArray();
        }


        public static Receta[] ReportLocalidadSinDespachar (Localidad localidad, MyDbContext _context)
        {
            string query = $"EXEC Receta_Report_Localidad_Despachada @localidad='{localidad}', @despachada=0";
            return _context.Recetas.FromSql(query).ToArray();
        }


        public static Receta[] GetAllByDoctorByDate(Fecha fecha, int doctor, MyDbContext _context)
        {
            string query = $"EXEC Receta_Report_Doctor @doctor={doctor}, @desde='{fecha.desde}', @hasta='{fecha.hasta}'";
            return _context.Recetas.FromSql(query).ToArray();
        }


        public static Receta[] GetAllByDoctorByStatus(int doctor, MyDbContext _context)
        {
            string query = $"EXEC Receta_Report_DoctorStatus @doctor={doctor}";
            return _context.Recetas.FromSql(query).ToArray();
        }


        public static void Delete(int id, MyDbContext _context)
        {
            string query = $"EXEC Receta_Delete @id={id}";
            _context.Database.ExecuteSqlCommand(query);
        }


        public static void UpdateDespachada(int id, MyDbContext _context)
        {
            string q = $"EXEC Receta_Update_despachada @idReceta={id}";
            _context.Database.ExecuteSqlCommand(q);
        }

        public static List<object> ReportByDoctor(Fecha fecha, int doctor, MyDbContext _context)
        {
            Receta[] recetas = GetAllByDoctorByDate(fecha, doctor, _context);
            var data = new List<object>();

            foreach (Receta receta in recetas) {
                var items = ItemReceta.ReportReceta(receta.id, _context);

                if (items.Count() > 0)
                    data.Add(new { receta = receta, items = items });
            }

            return data;
        }

        public static List<object> ReportByDoctorByStatus(int doctor, MyDbContext _context)
        {
            Receta[] recetas = GetAllByDoctorByStatus(doctor, _context);
            var data = new List<object>();

            foreach (Receta receta in recetas) {
                var items = ItemReceta.ReportReceta(receta.id, _context);
                if (items.Count() > 0)
                    data.Add(new { receta = receta, items = items });
            }

            return data;
        }
    }
}
