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
        [Allow("asistenteSalud")]
        public IActionResult Get_transferencias_pendientes(Token token)
        {
            if (token is null)
                return Unauthorized();

            try
            {
                Transferencia[] data = new Transferencia().getPendientes(token.data.localidad, _context);
                return Ok(data);

            } catch (Exception e) {
                return BadRequest("Something broke");
            }
        }


        [HttpPut("transferencia")]
        [Allow("asistenteSalud, bodeguero")]
        public IActionResult Despacho_Transferencia(Token token, [FromBody]TransferenciaRequest req)
        {
            if (req is null)
                return BadRequest();
            if (token is null)
                return Unauthorized();

            try
            {
                new Transferencia().despachar(token.data.cedula, req, _context);
                return Ok();

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
        }
    }
}
