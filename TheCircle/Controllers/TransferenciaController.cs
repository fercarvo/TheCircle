using Microsoft.AspNetCore.Mvc;
using TheCircle.Util;
using System;
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
        public IActionResult Get_transferencias_pendientes(Token token)
        {
            Transferencia[] data = new Transferencia().getPendientes(token.data.localidad, _context);
            return Ok(data);
        }


        [HttpPut("transferencia")]
        [APIauth("asistenteSalud, bodeguero")]
        public IActionResult Despacho_Transferencia(Token token, [FromBody]TransferenciaRequest req)
        {
            if (req is null)
                return BadRequest();

            new Transferencia().despachar(token.data.cedula, req, _context);
            return Ok();
        }
    }
}
