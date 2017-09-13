using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using System;
using TheCircle.Util;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class AtencionController : Controller
    {
        private readonly MyDbContext _context;
        public AtencionController (MyDbContext context)
        {
            _context = context;
        }

        //Crea una atencion medica
        [HttpPost ("atencion")]
        [APIauth("medico")]
        public IActionResult PostAtencion(Token token, [FromBody] AtencionRequest request)
        {
            if (request is null)
                return BadRequest();

            Atencion atencion = Atencion.New(request, token.data.cedula, token.data.localidad, _context);
            Diagnostico[] diagnosticos = Diagnostico.ReportByAtencion(atencion.id, _context);

            var response = new AtencionResponse(atencion, diagnosticos);
            return Ok(response);
        }


        //Crea una atencion medica
        [HttpPost("atencion2")]
        [APIauth("medico")]
        public IActionResult PostAtencion(Token token, [FromBody] AtencionRequest request)
        {
            if (request is null)
                return BadRequest();

            var atencion = new Atencion(request, token.data.cedula, token.data.localidad, _context);
            var diagnosticos = Diagnostico.ReportByAtencion(atencion.id, _context);

            return Ok(new {atencion = atencion, diagnosticos = diagnosticos});
        }


        //Ruta que retorna las atenciones medicas de un doctor
        [HttpGet("atencion/medico")]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)]
        [APIauth("medico")]
        public IActionResult Get_ReporteAtencion(Token token, [FromQuery] Fecha request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            Atencion[] atenciones = Atencion.ReportByDoctorDate(request, token.data.cedula, _context);
            return Ok(atenciones);
        }
    }
}
