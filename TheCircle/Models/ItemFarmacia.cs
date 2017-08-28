using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class ItemFarmacia
    {
        [Key]
        public int id { get; set; }
        public string nombre { get; set; }
        public string compuesto { get; set; }
        public string categoria { get; set; }
        public string grupo { get; set; }
        public int stock { get; set; }
        public DateTime fcaducidad { get; set; }

        public ItemFarmacia() { }

        public ItemFarmacia[] getAllByLocalidad (Localidad localidad, MyDbContext _context)
        {
            string query = $"EXEC dbo.select_ItemFarmacia @localidad='{localidad}'";

            try {
                var data = _context.ItemFarmacias.FromSql(query).ToArray();
                return data;
            } catch (Exception e) {
                Console.WriteLine(e);
                throw new Exception("Error cargar ItemsFarmacia, ItemFarmacia.getAllByLocalidad");
            }
        }

        public void insert(RequestItem item, Localidad localidad, int personal, MyDbContext _context) {
            string query = $"EXEC dbo.ItemFarmacia_insert @nombre='{item.nombre}', @compuesto='{item.compuesto}' , @fcaducidad='{item.fcaducidad}' , @cantidad={item.cantidad} ,@localidad='{localidad}' , @personal={personal} ";

            try
            {
                _context.Database.ExecuteSqlCommand(query);
            }
            catch (Exception e)
            {
                throw new Exception("Error al crear ItemsFarmacia at ItemFarmacia.insert");
            }
        }
    }

    public class Item
    {
        public int id { get; set; }
        public string nombre { get; set; }

        public Item() { }

        public Item[] getBy_Compuesto(string compuesto, MyDbContext _context)
        {
            string query = $"EXEC dbo.ItemFarmacia_Select_ByCompuesto @compuesto='{compuesto}'";

            try {
                var data = _context.ItemNombre.FromSql(query).ToArray();
                return data;
            } catch (Exception e) {
                throw new Exception("Error cargar Item at Item.getBy_Compuesto");
            }

        }
    }

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

        public List<Compuesto> getAllBy_Localidad(Localidad localidad, MyDbContext _context) {
            string query = $"EXEC dbo.Compuesto_Select_ByLocalidad @localidad='{localidad}'";

            try {
                var compuestos = new List<Compuesto>();
                var i = new Item();
                var data = _context.CompuestoNombre.FromSql(query).ToArray();
                
                foreach (CompuestoNombre compuesto in data) {
                    var items = i.getBy_Compuesto(compuesto.nombre, _context);

                    if (items.Count() > 0) 
                        compuestos.Add(new Compuesto(compuesto.nombre, items));
                }

                return compuestos;
            } catch (Exception e) {
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

    public class RequestItem
    {
        public string nombre { get; set; }
        public string compuesto { get; set; }
        public string fcaducidad { get; set; }
        public int cantidad { get; set; }

        public RequestItem() { }

    }
}
