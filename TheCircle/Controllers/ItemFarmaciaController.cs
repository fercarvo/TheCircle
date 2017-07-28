using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using Microsoft.EntityFrameworkCore;

namespace TheCircle.Controllers
{
    
    public class ItemFarmaciaController : Controller
    {
        private readonly MyDbContext _context;
        public ItemFarmaciaController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/ItemFarmacia
        [HttpGet("api/itemfarmacia/{localidad}")]
        public IEnumerable<ItemFarmacia> GetItems(string localidad)
        {
            //using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                string query = "EXEC dbo.select_ItemFarmacia @localidad=" + localidad;
                var data = _context.ItemFarmacias.FromSql(query).ToList();
                return data;
            }
        }
        
    }
}