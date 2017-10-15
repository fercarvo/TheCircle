using Microsoft.AspNetCore.Mvc;
using TheCircle.Util;
using TheCircle.Models;
using System;

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

        //Obtengo todos los pedido interno que tiene que despachar el asistente salud
        [HttpGet("pedidointerno/pendientes")]
        [APIauth("asistenteSalud")]
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Client)]
        public IActionResult GetPendientes(Token token)
        {
            var data = PedidoInterno.GetPendientes(token.data.localidad);
            return Ok(data);
        }

        //Todos los pedidos internos con inconsistencias
        [HttpGet("pedidointerno/inconsistentes")]
        [APIauth("contralor")]
        public IActionResult GetInconsistentes()
        {
            var data = PedidoInterno.GetInconsistentes();
            return Ok(data);
        }

        //Son los pedido interno que han sido despachados por el asistente pero falta la recepción del médico
        [HttpGet("pedidointerno/despachadas")]
        [APIauth("medico")]
        public IActionResult GetDespachadas(Token token)
        {
            PedidoInterno[] data = PedidoInterno.GetPendientesRecepcion(token.data.cedula);
            return Ok(data);
        }

        /*[HttpGet("pedidointerno/receptadas")]
        [APIauth("asistenteSalud")]
        public IActionResult GetReceptadas(Token token)
        {
            //PedidoInterno[] data = PedidoInterno.GetReceptadas(token.data.localidad, _c);
            return Ok();
        }*/

        //Genera un nuevo pedido interno
        [HttpPost("pedidointerno")]
        [APIauth("medico")]
        public IActionResult New(Token token, [FromBody] ItemFarmacia.Data req)
        {
            new PedidoInterno(req.item, req.cantidad, token.data.cedula);
            return Ok();
        }

        //Se despacha un pedido interno
        [HttpPut("pedidointerno/{id}/despachar")]
        [APIauth("asistenteSalud")]
        public IActionResult Despachar(Token token, int id, [FromBody] PedidoInterno.Data req)
        {
            PedidoInterno.Despachar(id, token.data.cedula, req.cantidad, req.comentario);
            return Ok();
        }

        //Se recepta un pedido interno por parte del medico
        [HttpPut("pedidointerno/{id}/receptar")]
        [APIauth("medico")]
        public IActionResult Receptar(Token token, int id, [FromBody] Comentario req)
        {
            PedidoInterno.Recepcion(id, req.comentario, token.data.cedula);
            return Ok();
        }

        public class Comentario {
            public String comentario { get; set; }
        }
    }
}
