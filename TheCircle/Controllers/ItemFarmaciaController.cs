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
        public IActionResult GetItems(string localidad)
        {
            ItemFarmacia item = new ItemFarmacia();
            ItemFarmacia[] stock = item.getAllByLocalidad(localidad, _context);
            if (stock != null) {
                return Ok(stock);
            } else {
                return BadRequest(stock);
            }
        }

    }
}
