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
        public RemisionController(MyDbContext context)
        {
            _context = context;
        }

        //Crea una remision medica
        [HttpPost("remision")]
        [APIauth("medico")]
        public IActionResult PostRemision([FromBody] Remision.Request req)
        {
            if (req is null)
                return BadRequest("Incorrect Data");

            new Remision(req.atencionM, req.institucion, req.monto, req.sintomas);
            return Ok();
        }


        //ruta que retorna las remisiones medicas de un doctor por rango de fechas
        [HttpGet("reporte/remision/date")]
        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Client)]
        [APIauth("medico")]
        public IActionResult Get_ReporteRemision_Date_Doctor(Token token, [FromQuery] Fecha request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Incorrect data");

            Remision[] response = Remision.ReportByDoctorDate(request, token.data.cedula);
            return Ok(response);
        }

        [HttpGet("remision/aprobacion1")]
        [APIauth("contralor")]
        public IActionResult ReportAP1()
        {
            var data = Remision.ReportAprobacion1();
            return Ok(data);
        }


        [HttpPost("remision/{id}/aprobacion1")]
        [APIauth("coordinador")]
        public IActionResult AprobadasAP1(Token token, int id, [FromBody]Aprobacion1 req)
        {
            var AP1 = new Remision.Aprobacion(id, req.monto, req.comentario, token.data.cedula);
            return Ok(AP1);
        }

        [HttpGet("remision")]
        [APIauth("contralor", "coordinador")]
        public IActionResult GetAll()
        {
            Remision[] remisiones = Remision.GetPendientes();

            return Ok(remisiones);
        }

        public class Aprobacion1
        {
            public Double monto { get; set; }
            public string comentario { get; set; } = null;
        }
    }
}