using Microsoft.AspNetCore.Mvc;
using System;
using TheCircle.Models;
using TheCircle.Util;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class CompuestoController : Controller
    {
        private readonly MyDbContext _context;
        public CompuestoController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("compuesto")]
        [APIauth("medico", "asistenteSalud", "bodeguero")]
        public IActionResult GetAll()
        {
            Compuesto[] compuestos = Compuesto.Report();
            return Ok(compuestos);
        }

        [HttpGet("compuesto-categoria-unidades")]
        [APIauth("bodeguero")]
        public IActionResult Get()
        {
            Compuesto[] compuestos = Compuesto.Report();
            Categoria[] categorias = Categoria.Report();
            UnidadMedida[] unidades = UnidadMedida.Report();

            return Ok(new { compuestos, categorias, unidades});
        }


        [HttpPost("compuesto")]
        [APIauth("bodeguero")]
        public IActionResult New([FromBody] Compuesto.Data req)
        {
            new Compuesto(req.nombre, req.categoria, req.unidad);
            return Ok();
        }
    }
}
