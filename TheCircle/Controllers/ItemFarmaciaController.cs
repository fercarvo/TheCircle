using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TheCircle.Models;
using TheCircle.Util;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class ItemFarmaciaController : Controller
    {
        private readonly MyDbContext _context;

        public ItemFarmaciaController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("itemfarmacia")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        [APIauth("medico", "asistenteSalud")]
        public IActionResult GetItems(Token token)
        {
            ItemFarmacia[] stock = ItemFarmacia.ReportLocalidad(token.data.localidad, _context);
            return Ok(stock);
        }



        [HttpPost("itemfarmacia")]
        [APIauth("asistenteSalud", "bodeguero")]
        public IActionResult PostItem(Token token, [FromBody]RequestItem item)
        {
            if (item is null)
                return BadRequest();
            
            ItemFarmacia.New(item, token.data.localidad, token.data.cedula, _context);

            return Ok();
        }
    }    
}
