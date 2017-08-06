using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAtenciones([FromBody] ReporteRequest request)
        {
            ReporteAtencion e = new ReporteAtencion();

            if (request != null) {
                ReporteAtencion[] response = e.getAll(request, _context);
                if (response != null) {
                    return Ok(response);
                } else {
                    return NotFound();
                }
            } else {
                return BadRequest();
            }
        }

        [HttpPost("remision")]
        public IActionResult GetAtenciones([FromBody] ReporteRequest request)
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
        public IActionResult GetAtenciones([FromBody] ReporteRequest request)
        {
            ReporteReceta e = new ReporteReceta();

            if (request != null) {
                ReporteReceta[] response = e.getAll(request, _context);
                if (response != null) {
                    return Ok(response);
                } else {
                    return NotFound();
                }
            } else {
                return BadRequest();
            }
        }
    }
}
