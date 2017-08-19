using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TheCircle.Models;

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
        public IActionResult getAllBy_Asistente(int asistente)
        {

            if (asistente == 1) {
                return Ok();
            } else {
                return BadRequest("getAllBy_Asistente broke");
            }
        }

        [HttpPost("api/despacho/receta")]
        public IActionResult PostDespachoReceta([FromBody] DespachoRecetaRequest request)
        {
            ItemsDespachoRequest i = new ItemsDespachoRequest();
            ItemDespacho id = new ItemDespacho();

            if (request != null) {
                foreach(ItemsDespachoRequest item in request.items) {
                    i.insert(item, _context);
                }

                int success = id.update_RecetaDespachada(request.id, _context);

                if (success == 1) {
                    return Ok();
                } else {
                    return BadRequest("No se insertaron los datos");
                }

            }
            return BadRequest("Incorrect Data");
        }


    }
}
