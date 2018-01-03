using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheCircle.Util;
using Microsoft.EntityFrameworkCore;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api/reporte")]
    public class ReporteController : Controller
    {
        private readonly MyDbContext _context;
        public ReporteController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("egresos")]
        [APIauth("contralor")]
        public IActionResult GetAll([FromQuery]DateTime desde, [FromQuery]DateTime hasta)
        {
            if (desde == null || hasta == null)
                return BadRequest(new { desde, hasta });

            string query = $"EXEC Reporte_Egresos @desde='{desde}', @hasta='{hasta}'";
            var reportes = new MyDbContext().EgresoTotal.FromSql(query).ToArray();

            return Ok(reportes);
        }

        [HttpGet("ingresos")]
        [APIauth("contralor")]
        public IActionResult GetIng([FromQuery]DateTime desde, [FromQuery]DateTime hasta)
        {
            if (desde == null || hasta == null)
                return BadRequest(new { desde, hasta });

            string query = $"EXEC Reporte_Ingresos @desde='{desde}', @hasta='{hasta}'";
            var reportes = new MyDbContext().IngresoTotal.FromSql(query).ToArray();

            return Ok(reportes);
        }

    }
}