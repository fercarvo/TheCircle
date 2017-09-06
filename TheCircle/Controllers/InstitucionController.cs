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
        public InstitucionController (MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("institucion")]
        [ResponseCache(Duration = 60*60*48, Location = ResponseCacheLocation.Client)]
        [Allow("medico")]
        public IActionResult GetInstituciones(Token token)
        {
            if (token is null)
                return Unauthorized();

            try
            {
                Institucion[] instituciones = new Institucion().getAll(_context);
                return Ok(instituciones);
                
            } catch (Exception e) {
                return BadRequest("Something broke");
            }
        }
    }
}