using Microsoft.AspNetCore.Mvc;
using System;
using TheCircle.Models;
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
        [HttpPost("atencion")]
        [APIauth("medico")]
        public IActionResult PostAtencion(Token token, [FromBody] Atencion.Data request)
        {
            if (request is null)
                return BadRequest();

            var atencion = new Atencion(request, token.data.cedula, token.data.localidad, _context);

            try { //Envio de email a operador
                Apadrinado apadrinado = Apadrinado.Get(request.apadrinado, _context);
                UserSafe operador = UserSafe.GetByCargo("operador");

                var peso = (float)request.peso / (float)apadrinado.peso;
                var talla = (float)request.talla / (float)apadrinado.talla;

                if (peso < 0.8 || peso > 1.2)
                    new EmailTC().AlertaPesoTalla(operador.nombre, operador.email, request.apadrinado, request.peso, request.talla, $"{token.data.nombres} {token.data.apellidos}");
                else if (talla < 0.8 || talla > 1.2)
                    new EmailTC().AlertaPesoTalla(operador.nombre, operador.email, request.apadrinado, request.peso, request.talla, $"{token.data.nombres} {token.data.apellidos}");
            } catch (Exception e) {
                //Si hay error no pasa nada
            }
            
            var diagnosticos = Diagnostico.ReportByAtencion(atencion.id, _context);
            return Ok(new {atencion, diagnosticos});
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

        //Ruta que retorna las atenciones medicas de un doctor
        [HttpGet("atencion/report")]
        //[ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Client)]
        //[APIauth("medico")]
        public IActionResult GetReport([FromQuery] Date fecha)
        {
            Atencion.Stadistics[] data = Atencion.Report(fecha.desde, fecha.hasta);
            return Ok(data);
        }
    }
}
