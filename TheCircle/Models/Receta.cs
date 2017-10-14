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

        //En caso de un mal despacho se notifica al contralor
        public static void AlertaDespacho(int idReceta) {
            try {
                var despachos = ItemDespacho.GetByReceta(idReceta, new MyDbContext());
                var contralor = UserSafe.GetByCargo("contralor");

                foreach (var item in despachos) {
                    if (item.cantidadDespachada != item.cantidadRecetada) {
                        new EmailTC().RecetaErronea($"{contralor.nombre} {contralor.apellido}", contralor.email, idReceta);
                        break;
                    }
                }
            } catch (Exception e) { }            
        }

        public static Receta[] ReportAsistente(int asistente, DateTime desde, DateTime hasta, MyDbContext _context)
        {
            string query = $"EXEC Receta_ReportBy_Asistente @asistente={asistente}, @desde='{desde}' , @hasta='{hasta}'";
            return _context.Recetas.FromSql(query).ToArray();
        }

        public static Impresion Get(int id)
        {
            string query = $"EXEC Receta_Select @id={id}";
            return new MyDbContext().RecetaImpresion.FromSql(query).First();
        }

        public static Receta[] ReportLocalidadSinDespachar (Localidad localidad, MyDbContext _context)
        {
            string query = $"EXEC Receta_Report_Localidad_Despachada @localidad='{localidad}', @despachada=0";
            return _context.Recetas.FromSql(query).ToArray();
        }

        public static Receta[] ReportLocalidad(Localidad localidad, DateTime desde, DateTime hasta)
        {
            string query = $"EXEC Receta_Report_Localidad @localidad='{localidad}', @desde='{desde}', @hasta='{hasta}'";
            return new MyDbContext().Recetas.FromSql(query).ToArray();
        }

        public static Receta[] ReportInconsistente() {
            string query = "EXEC Receta_Report_Inconsistente";
            return new MyDbContext().Recetas.FromSql(query).ToArray();
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


        public class Impresion {
            [Key]
            public int id { get; set; }
            public DateTime fecha { get; set; }
            public String nombreApadrinado { get; set; }
            public int codigoApadrinado { get; set; }
            public String doctor { get; set; }
            public int cedulaDoctor { get; set; }
        }
    }
}
