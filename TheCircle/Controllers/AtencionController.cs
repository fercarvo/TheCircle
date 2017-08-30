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
            var response = new AtencionResponse();
            var temp = new Diagnostico();
            var atencion = new Atencion(); //atencion creada

            if (request == null)
                return BadRequest("Incorrect Data");


            try {
                Token token = _validate.check(Request, new string[] { "medico" });
                atencion = atencion.crear(request, token.data.cedula, token.data.localidad, _context);

                Diagnostico[] diagnosticos = temp.getAllByAtencion(atencion.id, _context);

                response.atencion = atencion;
                response.diagnosticos = diagnosticos;

                return Ok();

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }

        }


        //Ruta que retorna las atenciones medicas de un doctor
        [HttpGet("reporte/atencion/date")]
        //[ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_ReporteAtencion([FromQuery] Fecha request)
        {
            Atencion a = new Atencion();

            if (!ModelState.IsValid)
                return BadRequest("Incorrect data");

            try
            {
                Token token = _validate.check(Request, new string[] { "medico" });

                Atencion[] atenciones = a.getBy_doctor_date(request, token.data.cedula, _context);
                return Ok(atenciones);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
            
        }
    }
}
