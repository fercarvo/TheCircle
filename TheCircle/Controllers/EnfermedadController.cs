using System;
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

        // GET: api/Enfermedad
        [HttpGet("enfermedad")]
        [ResponseCache(Duration = 60*60*120, Location = ResponseCacheLocation.Client)] //cache de 60*60*120 segundos = 120 horas
        [APIauth("medico")]
        public IActionResult GetEnfermedades()
        {
            try
            {
                var data = _context.Enfermedades.FromSql("EXEC dbo.select_Enfermedad").ToArray();
                return Ok(data);
            } catch (Exception e) {
                return StatusCode(500);
            }

        }

        //Ruta que retorna listade de enfermedades mas comunes por centro comunitario
        [HttpGet("reporte/enfermedad/date")]
        //[ResponseCache(Duration = 60*60*3, Location = ResponseCacheLocation.Client)] //cache de 60*60*3 segundos, para evitar sobrecarga de la BDD
        [APIauth("medico")]
        public IActionResult Get_ReporteEnfermedades(Token token, [FromQuery] Fecha request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Incorrect data");

            try
            {
                var response = new ReporteEnfermedad().getAll(request, token.data.localidad, _context);
                return Ok(response);

            } catch (Exception e) {
                return StatusCode(500);
            }
        }
    }
}
