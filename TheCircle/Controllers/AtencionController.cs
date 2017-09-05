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
        private Token _validate;

        public AtencionController (MyDbContext context)
        {
            _context = context;
            _validate = new Token();
        }

        //Crea una atencion medica
        [HttpPost ("atencion")]
        public IActionResult PostAtencion([FromBody] AtencionRequest request)
        {
            if (request == null)
                return BadRequest("Incorrect Data");

            try {
                Token token = _validate.check(Request, new string[] { "medico" });

                Atencion atencion = new Atencion().crear(request, token.data.cedula, token.data.localidad, _context);
                Diagnostico[] diagnosticos = new Diagnostico().getAllByAtencion(atencion.id, _context);

                var response = new AtencionResponse(atencion, diagnosticos);

                return Ok(response);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
        }


        //Ruta que retorna las atenciones medicas de un doctor
        [HttpGet("atencion/medico")]
        //[ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_ReporteAtencion([FromQuery] Fecha request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Incorrect data");

            try
            {
                Token token = _validate.check(Request, new string[] { "medico" });

                Atencion[] atenciones = new Atencion().getBy_doctor_date(request, token.data.cedula, _context);
                return Ok(atenciones);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
            
        }
    }
}
