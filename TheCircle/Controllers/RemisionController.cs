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

        //ruta que retorna las remisiones medicas de un doctor por rango de fechas
        [HttpGet("remision/gastado")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
        [APIauth("medico")]
        public IActionResult ReporteGastado(Token token)
        {
            Remision.Monto data = Remision.GetMonto(token.data.cedula);
            return Ok(data);
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
        public IActionResult AprobarRemision(Token token, int id, [FromBody]Aprobacion1 req)
        {
            new Remision.Aprobacion(id, req.monto, req.comentario, token.data.cedula);
            return Ok();
        }

        [HttpPost("remision/{id}/aprobacioncontralor")]
        [APIauth("contralor")]
        public IActionResult AprobacionContralor(Token token, int id, [FromBody]string comentario)
        {
            if (id <= 0)
                return BadRequest();

            Remision.AprobacionContralor(token.data.cedula, id, comentario);
            return Ok();
        }


        [HttpGet("remision")]
        [APIauth("contralor", "coordinador")]
        public IActionResult GetAll()
        {
            Remision[] remisiones = Remision.GetPendientes();

            return Ok(remisiones);
        }

        //Obtengo todas las aprobaciones1 rechazadas por el contralor
        [HttpGet("remision/aprobacion1/rechazada")]
        [APIauth("coordinador")]
        public IActionResult GetRechazadas()
        {
            var remisiones = Remision.GetAP1Rechazadas();

            return Ok(remisiones);
        }

        //Se actualiza la aprobacion1 previamente rechazada
        [HttpPut("remision/aprobacion1/{id}")]
        [APIauth("coordinador")]
        public IActionResult ReAprobarRemision(Token token, int id, [FromBody]Aprobacion1 req)
        {
            Remision.ReAprobarAP1(token.data.cedula, id, req.comentario, req.monto);
            return Ok();
        }

        //Se rechaza una remision medica por parte del contralor
        [HttpPut("remision/aprobacion1/{id}/rechazar")]
        [APIauth("contralor")]
        public IActionResult RechazarAP1(Token token, int id, [FromBody]Aprobacion1 data)
        {
            Remision.RechazarAP1(id, data.comentario);

            return Ok();
        }

        public class Aprobacion1
        {
            public Double monto { get; set; }
            public string comentario { get; set; } = null;
        }
    }
}