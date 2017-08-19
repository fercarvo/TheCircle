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

            int despachada = 0; //Todas las recetas que esten sin despachar

            List<RecetaTotal> recetas = rt.getAllByLocalidadByStatus(localidad, despachada, _context);

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
            RecetaDespacho receta = new RecetaDespacho();
            ItemDespacho id = new ItemDespacho();

            if (request != null) {
                foreach(ItemsDespachoRequest item in request.items) {
                    i.insert(item, _context);
                }

                receta.idReceta = request.id;
                receta.items = id.getByReceta(request.id, _context);

                if (receta.items != null) {
                    return Ok(receta);
                } else {
                    return BadRequest("No se insertaron los datos");
                }

            }
            return BadRequest("Incorrect Data");
        }


    }
}
