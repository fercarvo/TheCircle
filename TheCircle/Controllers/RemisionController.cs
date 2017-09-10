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
        public IActionResult PostRemision([FromBody] RemisionRequest request)
        {
            if (request is null)
                return BadRequest("Incorrect Data"); 

            Remision.New(request, _context);
            return Ok();         
        }


        //ruta que retorna las remisiones medicas de un doctor por rango de fechas
        [HttpGet("reporte/remision/date")]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)]
        [APIauth("medico")]
        public IActionResult Get_ReporteRemision_Date_Doctor(Token token, [FromQuery] Fecha request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Incorrect data");

            Remision[] response = Remision.ReportByDoctorDate(request, token.data.cedula, _context);
            return Ok(response);
        }
    }
}