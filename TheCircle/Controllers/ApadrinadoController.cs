using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using Microsoft.EntityFrameworkCore;
using System;
using TheCircle.Util;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class ApadrinadoController : Controller
    {
        private readonly MyDbContext _context;
        public ApadrinadoController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("apadrinado/{cod}")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        [Allow("medico")]
        public IActionResult GetApadrinado(Token token, int cod)
        {
            if (token is null)
                return Unauthorized();

            try
            {
                Apadrinado data = new Apadrinado().get(cod, _context);
                return Ok(data);
            } catch (Exception e) {
                return NotFound();
            }
        }


        [HttpGet("apadrinado/{cod}/foto")]
        [ResponseCache(Duration = 60 * 60 * 48, Location = ResponseCacheLocation.Client)]
        [Allow("medico")]
        public IActionResult GetApadrinadoFoto(Token token, int cod)
        {
            if (token is null)
                return Unauthorized();

            string query = $"EXEC dbo.select_Apadrinado_foto @cod={cod}";

            try
            {
                var foto = _context.Fotos.FromSql(query).First();
                var image = System.IO.File.OpenRead($"\\\\Guysrv08\\aptifyphoto\\DPHOTO\\Images\\{foto.path}\\{foto.name}");
                return File(image, "image/jpeg");

            } catch (Exception e) {
                return Redirect("/images/ci.png");
            }
        }
    }
}
