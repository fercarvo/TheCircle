using Microsoft.AspNetCore.Mvc;
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
            RecetaTotal recetaTotal = new RecetaTotal();

            if (localidad != null) {                
                return Ok(recetaTotal.getAll(localidad, _context));
            } else {
                return BadRequest();
            }
        }


    }
}
