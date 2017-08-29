using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using System;
using TheCircle.Util;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class RemisionController : Controller
    {
        private readonly MyDbContext _context;
        private Token _validate;

        public RemisionController (MyDbContext context)
        {
            _context = context;
            _validate = new Token();
        }

        //Crea una remision medica
        [HttpPost ("remision")]
        public IActionResult PostRemision([FromBody] RemisionRequest request)
        {

            Remision remision = new Remision();

            if (request == null)
                return BadRequest("Incorrect Data"); 

            try {

                _validate.check(Request, new string[] {"medico"});

                remision = remision.crear(request, _context);
                return Ok(remision);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
            
        }


        //ruta que retorna las remisiones medicas de un doctor por rango de fechas
        [HttpGet("reporte/remision/date")]
        //[ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_ReporteRemision_Date_Doctor([FromQuery] ReporteRequest request)
        {
            ReporteRemision rr = new ReporteRemision();

            if (!ModelState.IsValid)
                return BadRequest("Incorrect data");

            try {

                Token token = _validate.check(Request, new string[] { "medico" });

                var response = rr.getAll_Doctor_Date(request, token.data.cedula, _context);
                return Ok(response);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
        }




    }
}