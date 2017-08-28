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
        public ReportesController(MyDbContext context)
        {
            _context = context;
        }

        //Ruta que retorna listade de enfermedades mas comunes por centro comunitario
        [HttpGet("enfermedad")]
        [ResponseCache(Duration = 60*60*3, Location = ResponseCacheLocation.Client)] //cache de 60*60*3 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_ReporteEnfermedades([FromQuery] ReporteRequest request)
        {
            ReporteEnfermedad re = new ReporteEnfermedad();

            if (ModelState.IsValid) {               

                try {
                    ReporteEnfermedad[] response = re.getAll(request, _context);
                    return Ok(response);
                } catch (Exception e) {
                    return NotFound("Something broke");
                }
            }
            return BadRequest("Incorrect data");
        }

        //Ruta que retorna las atenciones medicas de un doctor
        [HttpGet("atencion")]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_ReporteAtencion([FromQuery] ReporteRequest request)
        {
            Atencion a = new Atencion();

            if (ModelState.IsValid) {

                try {
                    Atencion[] atenciones = a.getBy_doctor_date(request, _context);
                    return Ok(atenciones);

                } catch (Exception e) {
                    return BadRequest("Something broke");
                }
            }
            return BadRequest("Incorrect data");
        }

        //ruta que retorna las remisiones medicas de un doctor
        [HttpGet("remision")]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_ReporteRemision([FromQuery] ReporteRequest request)
        {
            ReporteRemision rr = new ReporteRemision();

            if (ModelState.IsValid) {

                try
                {
                    ReporteRemision[] response = rr.getAll(request, _context);
                    return Ok(response);
                } catch (Exception e)
                {
                    return BadRequest("Something broke");
                }
            }
            return BadRequest("Incorrect data");
        }

        //Ruta que retorna las recetas medicas emitidas por un doctor
        [HttpGet("receta")]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_ReporteReceta([FromQuery] ReporteRequest request)
        {
            RecetaTotal rt = new RecetaTotal();
            List<RecetaTotal> recetas;

            if (ModelState.IsValid)
            {
                try
                {
                    recetas = rt.reporteByDoctor(request, _context);
                    return Ok(recetas);
                }
                catch (Exception e)
                {
                    return BadRequest("Something broke");
                }
            }
            return BadRequest("Incorrect data");
        }

        //Ruta que retorna las recetas medicas no eliminadas y no despachadas de un doctor.
        [HttpGet("receta/{doctor}")]
        [ResponseCache(Duration = 40, Location = ResponseCacheLocation.Client)] //cache de 40 segundos, para evitar sobrecarga de la BDD
        public IActionResult GetRecetasByDoctorByStatus(int doctor)
        {
            RecetaTotal rt = new RecetaTotal();
            List<RecetaTotal> recetas;

            try
            {
                recetas = rt.reporteByDoctorByStatus(doctor, _context);
                return Ok(recetas);
            }
            catch (Exception e)
            {
                return BadRequest("Something broke");
            }
        }
    }
}
