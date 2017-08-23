using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TheCircle.Models;

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

        [HttpPost("enfermedad")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)] //cache de 30 segundos
        public IActionResult GetEnfermedades([FromBody] ReporteRequest request)
        {
            ReporteEnfermedad e = new ReporteEnfermedad();

            if (request != null) {
                ReporteEnfermedad[] response = e.getAll(request, _context);
                if (response != null) {
                    return Ok(response);
                } else {
                    return NotFound();
                }
            } else {
                return BadRequest();
            }
        }

        [HttpPost("atencion")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)] //cache de 30 segundos
        public IActionResult GetAtenciones([FromBody] ReporteRequest request)
        {
            Atencion a = new Atencion();

            if (request != null) {

                try {
                    Atencion[] atenciones = a.getBy_doctor_date(request, _context);
                    return Ok(atenciones);

                } catch (Exception e) {
                    return BadRequest("Something broke");
                }
            } else {
                return BadRequest("Incorrect data");
            }
        }

        [HttpPost("remision")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        public IActionResult GetRemisiones([FromBody] ReporteRequest request)
        {
            ReporteRemision e = new ReporteRemision();

            if (request != null) {
                ReporteRemision[] response = e.getAll(request, _context);
                if (response != null) {
                    return Ok(response);
                } else {
                    return NotFound();
                }
            } else {
                return BadRequest();
            }
        }

        [HttpPost("receta")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        public IActionResult GetRecetasByDoctorByDate([FromBody] ReporteRequest request)
        {
            RecetaTotal rt = new RecetaTotal();
            List<RecetaTotal> recetas;

            if (request != null) {
                recetas = rt.reporteByDoctor(request, _context);
                if (recetas != null) {
                    return Ok(recetas);
                } else {
                    return NotFound();
                }
            } else {
                return BadRequest();
            }
        }

        [HttpGet("receta/{doctor}")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        public IActionResult GetRecetasByDoctorByStatus(int doctor)
        {
            RecetaTotal rt = new RecetaTotal();
            List<RecetaTotal> recetas;

            recetas = rt.reporteByDoctorByStatus(doctor, _context);
            if (recetas != null) {
                return Ok(recetas);
            } else {
                return NotFound();
            }
        }
    }
}
