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

        // GET: api/Reportes
        [HttpPost("enfermedad")]
        public IActionResult GetEnfermedades([FromBody] ReporteEnfermedadRequest request)
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

        // GET: api/Reportes
        [HttpPost("atencion")]
        public IActionResult GetAtenciones([FromBody] ReporteAtencionRequest request)
        {
            ReporteAtencion e = new ReporteAtencion();

            if (request != null)
            {
                ReporteAtencion[] response = e.getAll(request, _context);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
