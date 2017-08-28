using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TheCircle.Models;
using TheCircle.Util;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api/reporte")]
    public class ReportesController : Controller
    {
        private readonly MyDbContext _context;
        private Token _validate;

        public ReportesController(MyDbContext context)
        {
            _context = context;
            _validate = new Token();
        }

        //Ruta que retorna listade de enfermedades mas comunes por centro comunitario
        [HttpGet("enfermedad/date")]
        //[ResponseCache(Duration = 60*60*3, Location = ResponseCacheLocation.Client)] //cache de 60*60*3 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_ReporteEnfermedades([FromQuery] ReporteRequest request)
        {
            ReporteEnfermedad re = new ReporteEnfermedad();

            if (!ModelState.IsValid)
                return BadRequest("Incorrect data");

            try {

                Token token = _validate.check(Request, new string[] { "medico" });

                var response = re.getAll(request, token.data.localidad, _context);
                return Ok(response);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
            
        }

        //Ruta que retorna las atenciones medicas de un doctor
        [HttpGet("atencion/date")]
        //[ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_ReporteAtencion([FromQuery] ReporteRequest request)
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

        //ruta que retorna las remisiones medicas de un doctor por rango de fechas
        [HttpGet("remision/date")]
        //[ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_ReporteRemision_Date_Doctor([FromQuery] ReporteRequest request)
        {
            ReporteRemision rr = new ReporteRemision();

            if (!ModelState.IsValid)
                return BadRequest("Incorrect data");

            try {

                Token token = _validate.check(Request, new string[] { "medico" });

                var response = rr.getAll_Doctor_Date(request, token.data.cedula, _context);
                return Ok(response);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }

        }

        //Ruta que retorna las recetas medicas emitidas por un doctor
        [HttpGet("receta/date")]
        //[ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_ReporteReceta_Doctor_Date([FromQuery] ReporteRequest request)
        {
            RecetaTotal rt = new RecetaTotal();
            List<RecetaTotal> recetas;

            if (!ModelState.IsValid)
                return BadRequest("Incorrect data");

            try {

                Token token = _validate.check(Request, new string[] { "medico" });

                recetas = rt.reporteByDoctor(request, token.data.cedula, _context);
                return Ok(recetas);
            }
            catch (Exception e)
            {
                return BadRequest("Something broke");
            }
            
        }

        //Ruta que retorna las recetas medicas no eliminadas y no despachadas de un doctor.
        [HttpGet("receta")]
        //[ResponseCache(Duration = 40, Location = ResponseCacheLocation.Client)] //cache de 40 segundos, para evitar sobrecarga de la BDD
        public IActionResult GetRecetasByDoctorByStatus()
        {
            RecetaTotal rt = new RecetaTotal();
            List<RecetaTotal> recetas;

            try {

                Token token = _validate.check(Request, new string[] { "medico" });

                recetas = rt.reporteByDoctorByStatus(token.data.cedula, _context);
                return Ok(recetas);
            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
        }
    }
}
