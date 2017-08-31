using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using System;
using TheCircle.Util;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class InstitucionController : Controller
    {
        private readonly MyDbContext _context;
        private Token _validate;

        public InstitucionController (MyDbContext context)
        {
            _context = context;
            _validate = new Token();
        }

        [HttpGet("institucion")]
        [ResponseCache(Duration = 60 * 60 * 48, Location = ResponseCacheLocation.Client)] //cache de 60 * 60 * 48 segundos = 48 horas
        public IActionResult GetInstituciones()
        {
            try {

                _validate.check(Request, new string[] {"medico"});

                Institucion[] instituciones = new Institucion().getAll(_context);
                return Ok(instituciones);
                
            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
        }




    }
}