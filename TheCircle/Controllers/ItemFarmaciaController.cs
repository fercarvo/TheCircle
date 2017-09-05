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
        private Token _validate;

        public ItemFarmaciaController(MyDbContext context)
        {
            _context = context;
            _validate = new Token();
        }

        [HttpGet("itemfarmacia")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        public IActionResult GetItems()
        {
            try {

                Token token = _validate.check(Request, new[] {"medico", "asistenteSalud"});

                ItemFarmacia[] stock = new ItemFarmacia().getAllByLocalidad(token.data.localidad, _context);
                return Ok(stock);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
        }

        [HttpGet("itemnombre")]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)] //cache de 1 hora
        public IActionResult GetNombres()
        {
            try {

                var token = _validate.check(Request, new[] {"asistenteSalud"});

                List<Compuesto> compuestos = new Compuesto().getAllBy_Localidad(token.data.localidad, _context);
                return Ok(compuestos);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
        }

        [HttpPost("itemfarmacia")]
        public IActionResult PostItem([FromBody]RequestItem item)
        {
            if (item == null)
                return BadRequest();

            try {
                
                Token token = _validate.check(Request, new[] {"asistenteSalud", "bodeguero"});

                new ItemFarmacia().insert(item, token.data.localidad, token.data.cedula, _context);
                return Ok();

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
        }
    }
    
}
