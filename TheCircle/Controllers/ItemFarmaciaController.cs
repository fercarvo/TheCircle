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
        [Allow("medico", "asistenteSalud")]
        public IActionResult GetItems(Token token)
        {
            if (token is null)
                return Unauthorized();

            try
            {
                ItemFarmacia[] stock = new ItemFarmacia().getAllByLocalidad(token.data.localidad, _context);
                return Ok(stock);

            } catch (Exception e) {
                return BadRequest("Something broke");
            }
        }

        [HttpGet("itemnombre")]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)] //cache de 1 hora
        [Allow("asistenteSalud")]
        public IActionResult GetNombres(Token token)
        {
            if (token is null)
                return Unauthorized();

            try
            {
                List<Compuesto> compuestos = new Compuesto().getAllBy_Localidad(token.data.localidad, _context);
                return Ok(compuestos);

            } catch (Exception e) {
                return BadRequest("Something broke");
            }
        }

        [HttpPost("itemfarmacia")]
        [Allow("asistenteSalud", "bodeguero")]
        public IActionResult PostItem(Token token, [FromBody]RequestItem item)
        {
            if (token is null)
                return Unauthorized();
            if (item is null)
                return BadRequest();

            try
            {                
                new ItemFarmacia().insert(item, token.data.localidad, token.data.cedula, _context);
                return Ok();

            } catch (Exception e) {
                return BadRequest("Something broke");
            }
        }
    }    
}
