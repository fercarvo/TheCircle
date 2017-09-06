using Microsoft.AspNetCore.Mvc;
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
        [Allow("medico", "asistente")]
        public IActionResult Get_Compuestos(Token token)
        {
            if (token is null)
                return Unauthorized();

            Compuesto2[] compuestos = new Compuesto2().getAll(_context);
            return Ok(compuestos);
        }
    }
}
