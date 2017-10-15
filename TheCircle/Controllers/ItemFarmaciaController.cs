using Microsoft.AspNetCore.Mvc;
using System;
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

        //Obtengo todos los items de farmacia que existen en cierta localidad
        [HttpGet("itemfarmacia")]
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Client)]
        [APIauth("medico", "asistenteSalud", "bodeguero")]
        public IActionResult GetItems(Token token)
        {
            ItemFarmacia[] stock = ItemFarmacia.ReportLocalidad(token.data.localidad, _context);
            return Ok(stock);
        }

        //Nombre de todos los items de farmacia registrados
        [HttpGet("itemfarmacia/nombre")]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)]
        [APIauth("asistenteSalud", "bodeguero")]
        public IActionResult GetNombres()
        {
            ItemFarmacia.Nombre[] nombres = ItemFarmacia.ReportNombres();
            return Ok(nombres);
        }

        //Retorna todos los items de farmacia que ha registrado un personal en específico, por rango de fecha
        [HttpGet("itemfarmacia/registro")]
        [APIauth("asistenteSalud", "contralor", "bodeguero")]
        public IActionResult GetRegistros(Token token, [FromQuery]Date fecha)
        {
            ItemFarmacia.Registro[] items = ItemFarmacia.ReportRegistro(token.data.cedula, fecha.desde, fecha.hasta);
            return Ok(items);
        }

        //Se obtiene todos los insumos medicos y odontológicos disponibles en cierta localidad
        [HttpGet("itemfarmacia/insumos")]
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        [APIauth("medico", "asistenteSalud")]
        public IActionResult GetItemsByTipo(Token token)
        {
            ItemFarmacia[] stock = ItemFarmacia.ReportLocalidadInsumos(token.data.localidad);
            return Ok(stock);
        }

        //Obtengo el stock de toda la fundacion menos de la localidad actual
        //SIN USO
        [HttpGet("itemfarmacia/report")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        [APIauth("medico", "asistenteSalud")]
        public IActionResult GetAll(Token token)
        {
            ItemFarmacia[] stock = ItemFarmacia.Report(token.data.localidad);
            return Ok(stock);
        }

        //Obtengo el stock de toda la fundacion
        [HttpGet("itemfarmacia/report/total")]
        [APIauth("coordinador", "bodeguero", "contralor")]
        public IActionResult GetStockGeneral(Token token)
        {
            ItemFarmacia[] stock = ItemFarmacia.ReportTotal();
            return Ok(stock);
        }

        //Obtengo un reporte de todos los itemsFarmacia que han sido alterados por el contralor
        [HttpGet("itemfarmacia/report/cambios")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
        [APIauth("contralor")]
        public IActionResult GetAlteraciones(Token token, [FromQuery]Date fecha)
        {
            var data = ItemFarmacia.ReportAlteraciones(token.data.cedula, fecha.desde, fecha.hasta);
            return Ok(data);
        }
        
        //Lista de todos los egresos por Receta médica que se han dato en cierta localidad, por rango de fecha
        [HttpGet("itemfarmacia/report/egresos")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        [APIauth("coordinadorCC")]
        public IActionResult GetEgresos(Token token, [FromQuery]Date req)
        {
            var stock = ItemFarmacia.Egresos(token.data.localidad, req.desde, req.hasta);
            return Ok(stock);
        }

        //Se crea un itemFarmacia desde una transferencia
        [HttpPost("itemfarmacia/transferencia")]
        [APIauth("asistenteSalud", "bodeguero")]
        public IActionResult PostItem(Token token, [FromBody]ItemFarmacia.IngresoTransferencia item)
        {
            if (item.idTransferencia <= 0)
                return BadRequest();

            new ItemFarmacia(item.idTransferencia, item.comentario, token.data.localidad, token.data.cedula);
            return Ok();
        }

        //Se crea un nuevo item farmacia para cierta localidad
        [HttpPost("itemfarmacia")]
        [APIauth("asistenteSalud", "bodeguero")]
        public IActionResult PostItem(Token token, [FromBody]ItemFarmacia.IngresoRequest item)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var data = new ItemFarmacia(item, token.data.localidad, token.data.cedula);
            return Ok(data);
        }

        //Se altera un item de farmacia, es decir su valor actual del stock
        [HttpPut("itemfarmacia/{id}")]
        [APIauth("contralor")]
        public IActionResult AlterarItem(Token token, int id, [FromQuery]int cantidad)
        {
            ItemFarmacia.Editar(id, token.data.cedula, cantidad);
            return Ok();
        }
    }    
}
