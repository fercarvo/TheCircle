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
        public RemisionController (MyDbContext context)
        {
            _context = context;
        }

        //Crea una remision medica
        [HttpPost ("remision")]
        [APIauth("medico")]
        public IActionResult PostRemision(Token token, [FromBody] RemisionRequest request)
        {
            if (token is null)
                return Unauthorized();
            if (request is null)
                return BadRequest("Incorrect Data"); 

            try
            {
                Remision remision = new Remision().crear(request, _context);
                return Ok(remision);

            } catch (Exception e) {
                return StatusCode(500);
            }
            
        }


        //ruta que retorna las remisiones medicas de un doctor por rango de fechas
        [HttpGet("reporte/remision/date")]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)]
        [APIauth("medico")]
        public IActionResult Get_ReporteRemision_Date_Doctor(Token token, [FromQuery] Fecha request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Incorrect data");

            try
            {
                ReporteRemision[] response = new ReporteRemision().getAll_Doctor_Date(request, token.data.cedula, _context);
                return Ok(response);

            } catch (Exception e) {
                return StatusCode(500);
            }
        }
    }
}