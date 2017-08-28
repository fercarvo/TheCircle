using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheCircle.Util;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api/enfermedad")]
    public class EnfermedadController : Controller
    {

        private readonly MyDbContext _context;
        public EnfermedadController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Enfermedad
        [HttpGet]
        [ResponseCache(Duration = 60*60*120, Location = ResponseCacheLocation.Client)] //cache de 60*60*120 segundos = 120 horas
        public IActionResult GetEnfermedades()
        {
            try {
                var data = _context.Enfermedades.FromSql("EXEC dbo.select_Enfermedad").ToArray();
                return Ok(data);
            } catch (Exception e) {
                return BadRequest("Something Broke");
            }

        }
    }
}
