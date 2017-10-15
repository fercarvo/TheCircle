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

        //Listado de instituciones para remision medica
        [HttpGet("institucion")]
        [ResponseCache(Duration = 60*60*48, Location = ResponseCacheLocation.Client)]
        [APIauth("medico")]
        public IActionResult GetInstituciones()
        {
            Institucion[] instituciones = Institucion.Report(_context);
            return Ok(instituciones);
        }
    }
}