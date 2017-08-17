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

        // GET: api/DespachoReceta
        [HttpGet("api/receta/{localidad}")]
        public IActionResult GetRecetasByLocalidad(string localidad) {
            RecetaTotal rt = new RecetaTotal();
            List<RecetaTotal> recetas = rt.getAllByLocalidad(localidad, _context);

            if (recetas != null) {                
                return Ok(recetas);
            } else {
                return BadRequest();
            }
        }

        [HttpPost("api/despacho/receta")]
        public IActionResult PostRemision([FromBody] DespachoRecetaRequest request)
        {
            ItemsDespachoRequest i = new ItemsDespachoRequest();

            if (request != null) {
                foreach(ItemsDespachoRequest item in request.items) {
                    i.insert(item, _context);
                }
                if () {

                }

            }
            return BadRequest("Incorrect Data");
        }


    }
}
