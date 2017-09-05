using System.Collections.Generic;
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
        private readonly Token _validate;
        public TransferenciaController(MyDbContext context)
        {
            _context = context;
            _validate = new Token();
        }


        [HttpGet("transferencia")]
        public IActionResult Get_transferencias_pendientes()
        {
            try
            {
                Token token = _validate.check(Request, new[] { "asistenteSalud" });

                Transferencia[] data = new Transferencia().getPendientes(token.data.localidad, _context);
                return Ok(data);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
        }


        [HttpPut("transferencia")]
        public IActionResult Despacho_Transferencia([FromBody]TransferenciaRequest req)
        {
            if (req == null)
                return BadRequest();

            try
            {
                Token token = _validate.check(Request, new[] { "asistenteSalud", "bodeguero" });

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
