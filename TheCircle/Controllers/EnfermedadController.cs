using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheCircle.Util;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class EnfermedadController : Controller
    {

        private readonly MyDbContext _context;
        private Token _validate;

        public EnfermedadController(MyDbContext context)
        {
            _context = context;
            _validate = new Token();
        }

        // GET: api/Enfermedad
        [HttpGet("enfermedad")]
        [ResponseCache(Duration = 60*60*120, Location = ResponseCacheLocation.Client)] //cache de 60*60*120 segundos = 120 horas
        public IActionResult GetEnfermedades()
        {
            try {

                _validate.check(Request, new string[] {"medico"});

                var data = _context.Enfermedades.FromSql("EXEC dbo.select_Enfermedad").ToArray();
                return Ok(data);
            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }

        }

                //Ruta que retorna listade de enfermedades mas comunes por centro comunitario
        [HttpGet("reporte/enfermedad/date")]
        //[ResponseCache(Duration = 60*60*3, Location = ResponseCacheLocation.Client)] //cache de 60*60*3 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_ReporteEnfermedades([FromQuery] Fecha request)
        {
            ReporteEnfermedad re = new ReporteEnfermedad();

            if (!ModelState.IsValid)
                return BadRequest("Incorrect data");

            try {

                Token token = _validate.check(Request, new string[] { "medico" });

                var response = re.getAll(request, token.data.localidad, _context);
                return Ok(response);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
            
        }
    }
}
