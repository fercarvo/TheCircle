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
        public IActionResult GetReportes([FromBody] EstadisticaEnfermedadReq request)
        {
            EstadisticaEnfermedad e = new EstadisticaEnfermedad();

            if (request != null) {
                EstadisticaEnfermedad[] response = e.getAll(request.desde, request.hasta, request.apadrinado, _context);
                if (response != null) {
                    return Ok(response);
                } else {
                    return NotFound();
                }
            } else {
                return BadRequest();
            }
        }

        
        // POST: api/Reportes
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Reportes/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
