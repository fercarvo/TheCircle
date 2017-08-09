using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TheCircle.Models
{
    public class Receta
    {
        [Key]
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fCaducidad { get; set; }
        public Boolean? despachada { get; set; }
        public Boolean? eliminada { get; set; }
        public int idDoctor { get; set; }
        public int idApadrinado { get; set; }

        public Receta () { }

        public Receta[] getAll(string localidad, MyDbContext _context) {
            string query = $"EXEC dbo.select_RecetaByLocalidad @localidad={localidad}";
            //try {
                var data = _context.Recetas.FromSql(query).ToArray();
                return data;
            //} catch (Exception e) {
              //  return null;
            //}
        }

        public Receta crear (RecetaRequest request, MyDbContext _context) {

            Receta receta;
            string query = $"DECLARE @id int EXEC dbo.insert_Receta @idDoctor={request.doctor}" +
              $", @idApadrinado={request.apadrinado}, @id = @id OUTPUT";

            try {
                receta = _context.Recetas.FromSql(query).First();
                return receta;
            } catch (Exception e) {
                return null;
            }
        }
    }

    public class RecetaRequest
    {
        public int doctor { get; set; }
        public int apadrinado { get; set; }

        public RecetaRequest() { }
    }

    public class RecetaTotal
    {
        public Receta receta { get; set; }
        public ItemReceta[] items { get; set; }

        public List <RecetaTotal> getAll(string localidad, MyDbContext _context) {

            Receta r = new Receta();
            ItemReceta i = new ItemReceta();

            Receta[] recetas = r.getAll(localidad, _context);            
            List<RecetaTotal> recetasTotales = new List<RecetaTotal>();

            foreach (Receta receta in recetas) { //se insertan en la base de datos todos los items
                ItemReceta[] items = i.getAllByReceta(receta.id, _context);
                if (items != null) {
                    recetasTotales.Add(new RecetaTotal(receta, items));
                }                
            }

            return recetasTotales;


        }

        public RecetaTotal() { }

        public RecetaTotal(Receta receta, ItemReceta[] items)
        {
            this.receta = receta;
            this.items = items;
        }
    }
}
