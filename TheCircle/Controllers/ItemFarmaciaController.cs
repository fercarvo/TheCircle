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
            try
            {
                ItemFarmacia[] stock = new ItemFarmacia().getAllByLocalidad(token.data.localidad, _context);
                return Ok(stock);

            } catch (Exception e) {
                return StatusCode(500);
            }
        }

        [HttpGet("itemnombre")]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)] //cache de 1 hora
        [APIauth("asistenteSalud")]
        public IActionResult GetNombres(Token token)
        {
            try
            {
                List<Compuesto> compuestos = new Compuesto().getAllBy_Localidad(token.data.localidad, _context);
                return Ok(compuestos);

            } catch (Exception e) {
                return StatusCode(500);
            }
        }

        [HttpPost("itemfarmacia")]
        [APIauth("asistenteSalud", "bodeguero")]
        public IActionResult PostItem(Token token, [FromBody]RequestItem item)
        {
            if (item is null)
                return BadRequest();

            try
            {                
                new ItemFarmacia().insert(item, token.data.localidad, token.data.cedula, _context);
                return Ok();

            } catch (Exception e) {
                return StatusCode(500);
            }
        }
    }    
}
