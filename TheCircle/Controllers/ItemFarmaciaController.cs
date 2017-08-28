using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TheCircle.Models;
using TheCircle.Util;

namespace TheCircle.Controllers
{

    public class ItemFarmaciaController : Controller
    {
        private readonly MyDbContext _context;
        private Token _validate;

        public ItemFarmaciaController(MyDbContext context)
        {
            _context = context;
            _validate = new Token();
        }

        // GET: api/ItemFarmacia
        [HttpGet("api/itemfarmacia/{localidad}")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        public IActionResult GetItems(string localidad)
        {
            ItemFarmacia item = new ItemFarmacia();

            try {
                ItemFarmacia[] stock = item.getAllByLocalidad(localidad, _context);
                return Ok(stock);
            } catch (Exception e) {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        [HttpGet("api/itemfarmacia")]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)] //cache de 1 hora
        public IActionResult GetNombres([FromQuery]string value) {

            var c = new Compuesto();
            var compuestos = new List<Compuesto>();

            if (value != "nombres")
                return BadRequest();

            try
            {
                var token = _validate.check(Request, "asistenteSalud");

                compuestos = c.getAllBy_Localidad(token.data.localidad, _context);
                return Ok(compuestos);
            }
            catch (Exception e)
            {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
        }

        [HttpPost("api/itemfarmacia")]
        public IActionResult PostItem([FromBody]RequestItem item)
        {
            var i = new ItemFarmacia();

            if (item == null)
                return BadRequest();

            try
            {
                var token = _validate.check(Request, "asistenteSalud");
                i.insert(item, token.data.localidad, token.data.cedula, _context);
                return Ok();
            }
            catch (Exception e)
            {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
        }
    }
    
}
