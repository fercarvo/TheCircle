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

            var remision = new Remision(req.atencionM, req.institucion, req.monto, req.sintomas);
            return Ok(remision);
        }

        //Obtengo una lista de todas las remisiones m�dicas aprobadas por contralor y coordinacion de salud
        [HttpGet("remision/aprobadas")]
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Client)]
        [APIauth("contralor", "coordinador")]
        public IActionResult GetAprobadas([FromQuery]Date fecha)
        {
            if (fecha is null)
                return BadRequest();

            var remisiones = Remision.GetAprobadas(fecha.desde, fecha.hasta);
            return Ok(remisiones);
        }

        //Retorna una vista de la remision creada
        [HttpGet("remision/{id}/imprimir")]
        [APIauth("medico")]
        public IActionResult GetRemision(Token token, int id)
        {
            if (id <= 0)
                return BadRequest("Incorrect data");

            try
            {
                Remision remision = Remision.Get(id);

                if (remision.cedulaDoctor != token.data.cedula)
                    return BadRequest("Usuario no autorizado ");

                string localidad = $"{token.data.localidad}";

                ViewData["Remision"] = remision;
                ViewData["Localidad"] = localidad;

                return View("~/Views/TheCircle/Impresion/Remision.cshtml");

            } catch (Exception e) {
                throw new Exception("Error al cargar remision", e);
            }
        }


        //ruta que retorna las remisiones medicas de un doctor por rango de fechas
        [HttpGet("reporte/remision/date")]
        [ResponseCache(Duration = 60 * 5, Location = ResponseCacheLocation.Client)]
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

        //reporte de todas las remisiones medicas que tienen la aprobacion 1 de coordinacion de salud
        [HttpGet("remision/aprobacion1")]
        [APIauth("contralor")]
        public IActionResult ReportAP1()
        {
            var data = Remision.ReportAprobacion1();
            return Ok(data);
        }

        //Se crea la aprobaci�n 1 para cierta remisi�n m�dica
        [HttpPost("remision/{id}/aprobacion1")]
        [APIauth("coordinador")]
        public IActionResult AprobarRemision(Token token, int id, [FromBody]Aprobacion1 req)
        {
            new Remision.Aprobacion(id, req.monto, req.comentario, token.data.cedula);
            return Ok();
        }

        //Se crea la aprobaci�n por parte del contralor
        [HttpPost("remision/{id}/aprobacioncontralor")]
        [APIauth("contralor")]
        public IActionResult AprobacionContralor(Token token, int id, [FromBody]string comentario)
        {
            if (id <= 0)
                return BadRequest();

            Remision.AprobacionContralor(token.data.cedula, id, comentario);
            return Ok();
        }

        //Obtengo todas las remisiones pendientes de aprobaci�n
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