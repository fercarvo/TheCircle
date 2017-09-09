using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheCircle.Util;
using TheCircle.Models;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class EnfermedadController : Controller
    {

        private readonly MyDbContext _context;
        public EnfermedadController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("enfermedad")]
        [ResponseCache(Duration = 3, Location = ResponseCacheLocation.Client)]
        [APIauth("medico")]
        public IActionResult GetEnfermedades()
        {
            var data = _context.Enfermedades.FromSql("EXEC Enfermedad_Report").ToArray();
            return Ok(data);
        }

        //Ruta que retorna listade de enfermedades mas comunes por centro comunitario
        [HttpGet("reporte/enfermedad/date")]
        [ResponseCache(Duration = 3, Location = ResponseCacheLocation.Client)]
        [APIauth("medico")]
        public IActionResult Get_ReporteEnfermedades(Token token, [FromQuery] Fecha request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Incorrect data");

            var response = new ReporteEnfermedad().getAll(request, token.data.localidad, _context);
            return Ok(response);
        }
    }
}
