using Microsoft.AspNetCore.Mvc;
using TheCircle.Util;
using TheCircle.Models;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class PedidoInternoController : Controller
    {
        private readonly MyDbContext _c;
        public PedidoInternoController(MyDbContext context)
        {
            _c = context;
        }

        [HttpGet("pedidointerno/pendientes")]
        [APIauth("asistenteSalud")]
        public IActionResult GetPendientes(Token token)
        {
            var data = PedidoInterno.GetPendientes(token.data.localidad, _c);
            return Ok(data);
        }

        [HttpGet("pedidointerno/despachadas")]
        [APIauth("asistenteSalud")]
        public IActionResult GetDespachadas(Token token)
        {
            PedidoInterno[] data = PedidoInterno.GetDespachadas(token.data.localidad, _c);
            return Ok(data);
        }

        [HttpGet("pedidointerno/receptadas")]
        [APIauth("asistenteSalud")]
        public IActionResult GetReceptadas(Token token)
        {
            PedidoInterno[] data = PedidoInterno.GetReceptadas(token.data.localidad, _c);
            return Ok(data);
        }

        [HttpPost("pedidointerno")]
        [APIauth("medico")]
        public IActionResult New(Token token, [FromBody] ItemFarmacia.Data req)
        {
            new PedidoInterno(req.item, req.cantidad, token.data.cedula);
            return Ok();
        }

        [HttpPut("pedidointerno/{id}/despachar")]
        [APIauth("asistenteSalud")]
        public IActionResult Despachar(Token token, int id, [FromBody] PedidoInterno.Data req)
        {
            PedidoInterno.Despachar(id, req, _c);
            return Ok();
        }

        [HttpPut("pedidointerno/{id}/receptar")]
        [APIauth("asistenteSalud")]
        public IActionResult Receptar(Token token, int id, [FromBody] string comentario)
        {
            PedidoInterno.Recepcion(id, comentario, token.data.cedula, _c);
            return Ok();
        }
    }
}
