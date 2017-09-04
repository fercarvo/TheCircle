using Microsoft.AspNetCore.Mvc;
using System;
using TheCircle.Models;
using TheCircle.Util;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class CompuestoController : Controller
    {
        private readonly MyDbContext _context;
        private readonly Token _validate;
        public CompuestoController(MyDbContext context)
        {
            _context = context;
            _validate = new Token();
        }


        [HttpGet("compuesto")]
        public IActionResult Get_Compuestos()
        {
            try {

                Compuesto2[] compuestos = new Compuesto2().getAll(_context);
                return Ok(compuestos);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
        }

    }
}
