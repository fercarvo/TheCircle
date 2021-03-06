using Microsoft.AspNetCore.Mvc;
using TheCircle.Util;
using TheCircle.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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
        [APIauth("asistenteSalud", "bodeguero")]
        public IActionResult GetPendientes(Token token)
        {
            Transferencia[] data = Transferencia.GetPendientes(token.data.localidad);
            return Ok(data);
        }

        [HttpGet("transferencia/coordinacion")]
        [APIauth("coordinador")]
        public IActionResult GetCreadas([FromQuery]DateTime desde, [FromQuery]DateTime hasta)
        {
            Transferencia[] data = Transferencia.GetCreadas(desde, hasta);
            return Ok(data);
        }

        [HttpGet("transferencia/despachada")]
        [APIauth("asistenteSalud")]
        public IActionResult GetDespachadas(Token token)
        {
            Transferencia[] data = Transferencia.GetDespachadas(token.data.localidad, _context);
            return Ok(data);
        }

        [HttpGet("transferencia/despachada/personal")]
        [APIauth("asistenteSalud", "bodeguero")]
        public IActionResult GetDespachadasByPersonal(Token token, [FromQuery]Date fecha)
        {
            Transferencia[] data = Transferencia.GetDespachadasByPersonal(token.data.cedula, fecha.desde, fecha.hasta);
            return Ok(data);
        }

        //Obtengo todas las transferencias de items que se han despachado con inconsistencia
        [HttpGet("transferencia/inconsistente")]
        [APIauth("contralor")]
        public IActionResult GetInconsistente()
        {
            Transferencia[] data = Transferencia.GetInconsistentes();
            /*UserSafe[] usuarios = UserSafe.GetAll();

            List <object> data2 = new List<object>();
            List<object> final = new List<object>();

            foreach (Transferencia transferencia in data) {
                dynamic obj = transferencia;
                obj.solicitante = "asdasda";
                obj.personalDespacho = "asdasd";
            }*/
            return Ok(data);
        }

        //Se crea una transferencia de farmacia
        [HttpPost("transferencia")]
        [APIauth("medico", "bodeguero", "coordinador")]
        public IActionResult New(Token token, [FromBody]ItemFarmacia.Data req)
        {
            if (req is null)
                return BadRequest();

            Localidad loc;

            if (Enum.TryParse(req.localidad, out loc))
                new Transferencia(req.item, loc, req.cantidad, token.data.cedula);
            else
                new Transferencia(req.item, token.data.localidad, req.cantidad, token.data.cedula);

            return Ok();
        }

        //Se despacha una transferencia solicitada
        [HttpPut("transferencia/{id}/despachar")]
        [APIauth("asistenteSalud", "bodeguero")]
        public IActionResult Despachar(Token token, int id, [FromBody]Transferencia.Data req)
        {
            if (req is null)
                return BadRequest();

            Transferencia.Despachar(token.data.cedula, req);
            return Ok();
        }


        //Se despacha una transferencia solicitada
        [HttpDelete("transferencia/{id}/")]
        [APIauth("coordinador")]
        public IActionResult Eliminar(int id)
        {
            if (id <= 0)
                return BadRequest();

            new MyDbContext().Database.ExecuteSqlCommand($"EXEC Transferencia_Delete @id={id}");

            return Ok();
        }
    }
}
