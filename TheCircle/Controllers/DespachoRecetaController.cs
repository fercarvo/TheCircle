using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TheCircle.Models;
using TheCircle.Util;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    public class DespachoRecetaController : Controller
    {
        private readonly MyDbContext _context;
        public DespachoRecetaController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("api/receta/{localidad}")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        public IActionResult GetRecetasByLocalidad(string localidad) {
            RecetaTotal rt = new RecetaTotal();
            int despachada = 0; //Todas las recetas que esten sin despachar
            List<RecetaTotal> recetas = rt.getAllByLocalidadByStatus(localidad, despachada, _context);

            if (recetas != null) {
                return Ok(recetas);
            } else {
                return BadRequest("GetRecetasByLocalidad broke");
            }
        }


        [HttpGet("api/despacho/receta/{asistente}")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        public IActionResult getRecetasDespachadas(int asistente) {

            RecetaDespacho rd = new RecetaDespacho();

            try {
                List<RecetaDespacho> recetas = rd.getBy_Asistente(asistente, _context);
                return Ok(recetas);
            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        [HttpPost("api/despacho/receta")]
        public IActionResult PostDespachoReceta([FromBody] DespachoRecetaRequest request) {
            ItemsDespachoRequest i = new ItemsDespachoRequest();
            ItemDespacho id = new ItemDespacho();

            if (request != null) {
                try {
                    foreach(ItemsDespachoRequest item in request.items) { //Se insertan todos los despachos
                        i.insert(item, _context);
                    }
                    id.update_RecetaDespachada(request.id, _context); //Se actualiza la receta a despachada
                    return Ok();
                } catch (Exception e) {
                    return BadRequest(e);
                }
            }
            return BadRequest("Incorrect Data");
        }
    }
}
