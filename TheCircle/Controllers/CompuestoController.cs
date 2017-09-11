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
        [APIauth("medico", "asistenteSalud", "bodeguero")]
        public IActionResult Get_Compuestos()
        {
            Compuesto[] compuestos = Compuesto.Report(_context);
            return Ok(compuestos);
        }
    }
}
