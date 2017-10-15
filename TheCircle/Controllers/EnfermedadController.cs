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

        //Listado de enfermedades registrados en la OMS
        [HttpGet("enfermedad")]
        [ResponseCache(Duration = 60*60*24*30, Location = ResponseCacheLocation.Client)] //30 dias de cache, demasiada información
        [APIauth("medico")]
        public IActionResult GetEnfermedades()
        {
            var data = _context.Enfermedades.FromSql("EXEC Enfermedad_Report").ToArray();
            return Ok(data);
        }

        //Ruta que retorna listade de enfermedades mas comunes por centro comunitario
        [HttpGet("reporte/enfermedad/date")]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)]
        [APIauth("medico")]
        public IActionResult Get_ReporteEnfermedades(Token token, [FromQuery] Fecha request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Incorrect data");

            var data = ReporteEnfermedad.Report(request, token.data.localidad, _context);
            return Ok(data);
        }
    }
}
