using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
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
            var diagnosticos = Diagnostico.ReportByAtencion(atencion.id, _context);

            if (request.peso > 0 || request.talla > 0)
                Task.Run( ()=> Atencion.AlertaPesoTalla(request, token) ); //Ejecución asyncrona sin espera, continua despues del return
            
            return Ok(new { atencion, diagnosticos });
        }


        //Ruta que retorna las atenciones medicas de un doctor
        [HttpGet("atencion/medico")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
        [APIauth("medico")]
        public IActionResult Get_ReporteAtencion(Token token, [FromQuery] Fecha request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            Atencion[] atenciones = Atencion.ReportByDoctorDate(request, token.data.cedula, _context);
            return Ok(atenciones);
        }

        //Ruta que retorna un reporte de las atenciones médicas por CC
        [HttpGet("atencion/report")]
        //[APIauth("director")]
        public IActionResult GetReport([FromQuery] Date fecha)
        {
            Atencion.Stadistics[] data = Atencion.Report(fecha.desde, fecha.hasta);
            return Ok(data);
        }

        //Ruta que retorna un reporte de todas las atenciones Médicas de Children
        [HttpGet("atencion")]
        [APIauth("coordinador")]
        public IActionResult GetAll([FromQuery] Date fecha)
        {
            Atencion.Reporte[] data = Atencion.getAtenciones(fecha.desde, fecha.hasta);
            return Ok(data);
        }
    }
}
