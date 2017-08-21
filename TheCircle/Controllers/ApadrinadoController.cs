using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api/apadrinado")]
    public class ApadrinadoController : Controller
    {

        private readonly MyDbContext _context;
        public ApadrinadoController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("{cod}")]
        [ResponseCache(Duration = 60)] //cache de 60 segundos
        public IActionResult GetApadrinado(int cod)
        {
            Apadrinado apadrinado = new Apadrinado();

            try {
                apadrinado = apadrinado.get(cod, _context);
                return Ok(apadrinado);
            } catch (Exception e) {
                return NotFound();
            }
        }


        [HttpGet("foto/{cod}")]
        [ResponseCache(Duration = 60 * 5)]
        public IActionResult GetApadrinadoFoto(int cod)
        {
            string query = $"EXEC dbo.select_Apadrinado_foto @cod={cod}";
            Foto foto;
            try {
                foto = _context.Fotos.FromSql(query).First();
                var image = System.IO.File.OpenRead($"\\\\Guysrv08\\aptifyphoto\\DPHOTO\\Images\\{foto.path}\\{foto.name}");
                return File(image, "image/jpeg");
            } catch (Exception e) {
                return Redirect("/images/ci.png");
            }
        }
    }
}
