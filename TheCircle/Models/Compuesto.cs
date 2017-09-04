using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class Compuesto
    {
        [Key]
        public string nombre { get; set; }
        public Item[] items { get; set; }

        public Compuesto() { }

        public Compuesto(string nombre, Item[] items)
        {
            this.nombre = nombre;
            this.items = items;
        }

        public List<Compuesto> getAllBy_Localidad(Localidad localidad, MyDbContext _context)
        {
            string query = $"EXEC dbo.Compuesto_Select_ByLocalidad @localidad='{localidad}'";

            try
            {
                var compuestos = new List<Compuesto>();
                var i = new Item();
                var data = _context.CompuestoNombre.FromSql(query).ToArray();

                foreach (CompuestoNombre compuesto in data)
                {
                    var items = i.getBy_Compuesto(compuesto.nombre, _context);

                    if (items.Count() > 0)
                        compuestos.Add(new Compuesto(compuesto.nombre, items));
                }

                return compuestos;
            }
            catch (Exception e)
            {
                throw new Exception("Error cargar Compuesto at ItemFarmacia.cs Compuesto.getAllBy_Localidad");
            }

        }

    }

    public class CompuestoNombre
    {
        [Key]
        public string nombre { get; set; }

        public CompuestoNombre() { }
    }

    public class Compuesto2
    {
        [Key]
        public string nombre { get; set; }
        public string unidadMedida { get; set; }
        public string categoriaNombre { get; set; }
        public string categoriaCodigo { get; set; }
        public string grupo { get; set; }

        public Compuesto2[] getAll(MyDbContext _context) {
            string query = $"EXEC dbo.Compuesto_Report";

            try
            {
                var data = _context.Compuesto2.FromSql(query).ToArray();
                return data;
            } catch (Exception e) {
                throw new Exception("Error cargar Compuesto2 at Compuesto.cs Compuesto2.getAll");
            }
        }

        public void crear() {

        }

    }

    public class CompuestoRequest
    {
        public string nombre { get; set; }
        public string cateroria { get; set; }
        public string unidad { get; set; }
    }
}
