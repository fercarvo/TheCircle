using Microsoft.AspNetCore.Mvc;
using TheCircle.Util;
using TheCircle.Models;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class TransferenciaController : Controller
    {
        private readonly MyDbContext _context;
        public TransferenciaController(MyDbContext context)
        {
            _context = context;
        }


        [HttpGet("transferencia")]
        [APIauth("asistenteSalud")]
        public IActionResult GetPendientes(Token token)
        {
            Transferencia[] data = Transferencia.GetPendientes(token.data.localidad, _context);
            return Ok(data);
        }

        [HttpGet("transferencia/despachada")]
        [APIauth("asistenteSalud")]
        public IActionResult GetDespachadas(Token token)
        {
            Transferencia[] data = Transferencia.GetDespachadas(token.data.localidad, _context);
            return Ok(data);
        }

        [HttpPost("transferencia")]
        [APIauth("medico", "bodeguero")]
        public IActionResult New(Token token, [FromBody]ItemFarmacia.Data req)
        {
            if (req is null)
                return BadRequest();

            new Transferencia(req.item, token.data.localidad, req.cantidad, token.data.cedula);
            return Ok();
        }


        [HttpPut("transferencia")]
        [APIauth("asistenteSalud", "bodeguero")]
        public IActionResult Despachar(Token token, [FromBody]Transferencia.Data req)
        {
            if (req is null)
                return BadRequest();

            Transferencia.Despachar(token.data.cedula, req, _context);
            return Ok();
        }
    }
}
