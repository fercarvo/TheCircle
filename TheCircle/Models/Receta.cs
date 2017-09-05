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

        public Receta[] getAllByLocalidad(Localidad localidad, MyDbContext _context)
        {
            string query = $"EXEC dbo.select_RecetaByLocalidad @localidad='{localidad}'";
            try {
                var data = _context.Recetas.FromSql(query).ToArray();
                return data;
            } catch (Exception e) {
                throw new Exception("Error al cargar recetas, Receta.getAllByLocalidad");
            }
        }

        public Receta[] getBy_Asistente(int asistente, MyDbContext _context)
        {
            string query = $"EXEC dbo.Receta_ReportBy_Asistente @asistente={asistente}";
            try {
                var data = _context.Recetas.FromSql(query).ToArray();
                return data;
            } catch (Exception e) {
                throw new Exception("Error al cargar recetas getBy_Asistente");
            }
        }

        public Receta[] getAll_Localidad_SinDespachar(Localidad localidad, MyDbContext _context)
        {
            string query = $"EXEC dbo.Receta_ReportBy_Localidad_Despachada @localidad='{localidad}', @despachada=0";
            try
            {
                var data = _context.Recetas.FromSql(query).ToArray();
                return data;
            }
            catch (Exception e)
            {
                throw new Exception("Error al cargar Receta[] at Receta.getAllByLocalidadByStatus");
            }
        }

        public Receta[] getAllByDoctorByDate(Fecha fecha, int doctor, MyDbContext _context)
        {
            string query = $"EXEC dbo.select_RecetaByDoctor @doctor={doctor}, @desde='{fecha.desde}', @hasta='{fecha.hasta}'";
            try {
                var data = _context.Recetas.FromSql(query).ToArray();
                return data;
            } catch (Exception e) {
                return null;
            }
        }

        public Receta[] getAllByDoctorByStatus(int doctor, MyDbContext _context)
        {
            string query = $"EXEC dbo.select_RecetaByDoctorByStatus @doctor={doctor}";
            try {
                var data = _context.Recetas.FromSql(query).ToArray();
                return data;
            } catch (Exception e) {
                return null;
            }
        }

        public Receta crear (int apadrinado, int doctor, MyDbContext _context) 
        {
            string query = $"DECLARE @id int EXEC dbo.insert_Receta @idDoctor={doctor}" +
              $", @idApadrinado={apadrinado}, @id = @id OUTPUT";

            try {
                var receta = _context.Recetas.FromSql(query).First();
                return receta;
            } catch (Exception e) {
                Console.WriteLine(e);
                throw new Exception("Error al crear/cargar receta medica");
            }
        }

        public void delete(int id, MyDbContext _context)
        {
            string query = $"EXEC dbo.delete_Receta @id={id}";

            try {
                _context.Database.ExecuteSqlCommand(query);
            } catch (Exception e) {
                Console.WriteLine(e);
                throw new Exception("Error borrar Receta, Receta.delete");
            }
        }

        internal void update_despachada(int id, MyDbContext _context)
        {
            string q = $"EXEC dbo.Receta_Update_despachada @idReceta={id}";
            try
            {
                _context.Database.ExecuteSqlCommand(q);
            } catch (Exception e) {
                throw;
            }
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
            var i = new ItemReceta();
            var recetasTotales = new List<RecetaTotal>();

            Receta[] recetas = new Receta().getAllByLocalidad(localidad, _context);

            foreach (Receta receta in recetas) {
                ItemReceta[] items = i.getAllByReceta(receta.id, _context);
                if (items != null) 
                    recetasTotales.Add(new RecetaTotal(receta, items));
            }
            return recetasTotales;
        }

        public RecetaTotal[] getAll_Localidad_SinDespachar(Localidad localidad, MyDbContext _context)
        {
            var i = new ItemReceta();

            var recetas = new Receta().getAll_Localidad_SinDespachar(localidad, _context);
            var recetasTotales = new List<RecetaTotal>();

            foreach (Receta receta in recetas) {
                var items = i.getAllByReceta(receta.id, _context);                 
                recetasTotales.Add(new RecetaTotal(receta, items));                    
            }
            return recetasTotales.ToArray();
        }

        public List<RecetaTotal> reporteByDoctor (Fecha fecha, int doctor, MyDbContext _context)
        {
            ItemReceta i = new ItemReceta();

            Receta[] recetas = new Receta().getAllByDoctorByDate(fecha, doctor, _context);
            List<RecetaTotal> recetasTotales = new List<RecetaTotal>();

            if (recetas != null) {
                foreach (Receta receta in recetas)
                { //se insertan en la base de datos todos los items

                    ItemReceta[] items = i.getAllByReceta(receta.id, _context);
                    if (items.Count() > 0)
                    {
                        recetasTotales.Add(new RecetaTotal(receta, items));
                    }
                }
            }

            return recetasTotales;
        }

        public List<RecetaTotal> reporteByDoctorByStatus(int doctor, MyDbContext _context)
        {
            ItemReceta i = new ItemReceta();

            Receta[] recetas = new Receta().getAllByDoctorByStatus(doctor, _context);
            List<RecetaTotal> recetasTotales = new List<RecetaTotal>();

            foreach (Receta receta in recetas)
            {

                ItemReceta[] items = i.getAllByReceta(receta.id, _context);
                if (items.Count() > 0)
                {
                    recetasTotales.Add(new RecetaTotal(receta, items));
                }
            }
            return recetasTotales;
        }
    }
}
