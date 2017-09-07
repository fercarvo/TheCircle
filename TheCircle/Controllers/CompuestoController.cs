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
        public CompuestoController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("compuesto")]
        [APIauth("medico", "asistente")]
        public IActionResult Get_Compuestos()
        {
            try
            {
                Compuesto2[] compuestos = new Compuesto2().getAll(_context);
                return Ok(compuestos);

            } catch (Exception e) {
                return StatusCode(500);
            }
        }
    }
}
