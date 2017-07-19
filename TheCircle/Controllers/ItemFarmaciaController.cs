using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using Microsoft.EntityFrameworkCore;

namespace TheCircle.Controllers
{
    [Route("api/ItemFarmacia")]
    public class ItemFarmaciaController : Controller
    {
        private readonly MyDbContext _context;
        public ItemFarmaciaController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/ItemFarmacia
        [HttpGet]
        public IEnumerable<ItemFarmacia> Get()
        {
            //using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                string query = "EXEC dbo.select_ItemFarmacia @localidad=" + 1;
                var data = _context.ItemFarmacias.FromSql(query).ToList();
                return data;
            }
        }
        
    }
}