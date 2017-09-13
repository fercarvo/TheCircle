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
            var q = $"EXEC Receta_Insert @doctor={doctor}, @apadrinado={apadrinado}";

            var receta = _context.Recetas.FromSql(q).First();
            id = receta.id;
        }

        public Receta[] getAllByLocalidad(Localidad localidad, MyDbContext _context)
        {
            string query = $"EXEC dbo.select_RecetaByLocalidad @localidad='{localidad}'";

            var data = _context.Recetas.FromSql(query).ToArray();
            return data;
        }

        public Receta[] getBy_Asistente(int asistente, MyDbContext _context)
        {
            string query = $"EXEC dbo.Receta_ReportBy_Asistente @asistente={asistente}";

            var data = _context.Recetas.FromSql(query).ToArray();
            return data;
        }

        public Receta[] getAll_Localidad_SinDespachar(Localidad localidad, MyDbContext _context)
        {
            string query = $"EXEC dbo.Receta_ReportBy_Localidad_Despachada @localidad='{localidad}', @despachada=0";

            var data = _context.Recetas.FromSql(query).ToArray();
            return data;
        }

        public static Receta[] GetAllByDoctorByDate(Fecha fecha, int doctor, MyDbContext _context)
        {
            string query = $"EXEC dbo.select_RecetaByDoctor @doctor={doctor}, @desde='{fecha.desde}', @hasta='{fecha.hasta}'";

            var data = _context.Recetas.FromSql(query).ToArray();
            return data;
        }

        public Receta[] getAllByDoctorByStatus(int doctor, MyDbContext _context)
        {
            string query = $"EXEC dbo.select_RecetaByDoctorByStatus @doctor={doctor}";

            var data = _context.Recetas.FromSql(query).ToArray();
            return data;
        }

        public static void Delete(int id, MyDbContext _context)
        {
            string query = $"EXEC dbo.delete_Receta @id={id}";

            _context.Database.ExecuteSqlCommand(query);
        }

        public static void UpdateDespachada(int id, MyDbContext _context)
        {
            string q = $"EXEC dbo.Receta_Update_despachada @idReceta={id}";

            _context.Database.ExecuteSqlCommand(q);
        }
    }

    public class RecetaTotal
    {
        public Receta receta { get; set; }
        public ItemReceta[] items { get; set; }

        public RecetaTotal() { }
        public RecetaTotal(Receta receta, ItemReceta[] items)
        {
            this.receta = receta;
            this.items = items;
        }

        public List<RecetaTotal> getAllByLocalidad (Localidad localidad, MyDbContext _context)
        {
            var recetasTotales = new List<RecetaTotal>();

            Receta[] recetas = new Receta().getAllByLocalidad(localidad, _context);

            foreach (Receta receta in recetas) {
                ItemReceta[] items = ItemReceta.GetAllByReceta(receta.id, _context);
                recetasTotales.Add(new RecetaTotal(receta, items));
            }

            return recetasTotales;
        }

        public RecetaTotal[] getAll_Localidad_SinDespachar(Localidad localidad, MyDbContext _context)
        {
            var recetas = new Receta().getAll_Localidad_SinDespachar(localidad, _context);
            var recetasTotales = new List<RecetaTotal>();

            foreach (Receta receta in recetas) {
                var items = ItemReceta.GetAllByReceta(receta.id, _context);                 
                recetasTotales.Add(new RecetaTotal(receta, items));                    
            }
            return recetasTotales.ToArray();
        }

        public static List<RecetaTotal> ReportByDoctor (Fecha fecha, int doctor, MyDbContext _context)
        {
            Receta[] recetas = Receta.GetAllByDoctorByDate(fecha, doctor, _context);
            List<RecetaTotal> recetasTotales = new List<RecetaTotal>();

            foreach (Receta receta in recetas) 
            { //se insertan en la base de datos todos los items
                ItemReceta[] items = ItemReceta.GetAllByReceta(receta.id, _context);

                if (items.Count() > 0)
                    recetasTotales.Add(new RecetaTotal(receta, items));
            }            

            return recetasTotales;
        }

        public List<RecetaTotal> reporteByDoctorByStatus(int doctor, MyDbContext _context)
        {
            Receta[] recetas = new Receta().getAllByDoctorByStatus(doctor, _context);
            List<RecetaTotal> recetasTotales = new List<RecetaTotal>();

            foreach (Receta receta in recetas) {

                ItemReceta[] items = ItemReceta.GetAllByReceta(receta.id, _context);
                if (items.Count() > 0)
                    recetasTotales.Add(new RecetaTotal(receta, items));
            }
            
            return recetasTotales;
        }
    }
}
