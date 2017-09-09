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

            Atencion atencion = new Atencion().crear(request, token.data.cedula, token.data.localidad, _context);
            Diagnostico[] diagnosticos = new Diagnostico().getAllByAtencion(atencion.id, _context);

            var response = new AtencionResponse(atencion, diagnosticos);
            return Ok(response);
        }


        //Ruta que retorna las atenciones medicas de un doctor
        [HttpGet("atencion/medico")]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)]
        [APIauth("medico")]
        public IActionResult Get_ReporteAtencion(Token token, [FromQuery] Fecha request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            Atencion[] atenciones = new Atencion().getBy_doctor_date(request, token.data.cedula, _context);
            return Ok(atenciones);
        }
    }
}
