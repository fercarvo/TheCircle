using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using TheCircle.Util;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class ItemFarmaciaController : Controller
    {
        private readonly MyDbContext _context;

        public ItemFarmaciaController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("itemfarmacia")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        [APIauth("medico", "asistenteSalud")]
        public IActionResult GetItems(Token token)
        {
            ItemFarmacia[] stock = ItemFarmacia.ReportLocalidad(token.data.localidad, _context);
            return Ok(stock);
        }

        [HttpGet("itemfarmacia/insumos")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        [APIauth("medico", "asistenteSalud")]
        public IActionResult GetItemsByTipo(Token token)
        {
            ItemFarmacia[] stock = ItemFarmacia.ReportLocalidadInsumos(token.data.localidad);
            return Ok(stock);
        }

        [HttpGet("itemfarmacia/report")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        [APIauth("medico", "asistenteSalud")]
        public IActionResult GetAll(Token token)
        {
            ItemFarmacia[] stock = ItemFarmacia.Report();
            return Ok(stock);
        }
        

        [HttpPost("itemfarmacia/transferencia")]
        [APIauth("asistenteSalud", "bodeguero")]
        public IActionResult PostItem(Token token, [FromBody]ItemFarmacia.IngresoTransferencia item)
        {
            if (item.idTransferencia <= 0)
                return BadRequest();

            new ItemFarmacia(item.idTransferencia, item.comentario, token.data.localidad, token.data.cedula);
            //ItemFarmacia.New(item, token.data.localidad, token.data.cedula, _context);
            return Ok();
        }


        [HttpPost("itemfarmacia")]
        [APIauth("asistenteSalud", "bodeguero")]
        public IActionResult PostItem(Token token, [FromBody]ItemFarmacia.Ingreso item)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var data = new ItemFarmacia(item, token.data.localidad, token.data.cedula);
            //ItemFarmacia.New(item, token.data.localidad, token.data.cedula, _context);
            return Ok(data);
        }
    }    
}
