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
        private readonly Token _validate;
        public ApadrinadoController(MyDbContext context)
        {
            _context = context;
            _validate = new Token();
        }

        [HttpGet("apadrinado/{cod}")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        public IActionResult GetApadrinado(int cod)
        {
            Apadrinado apadrinado = new Apadrinado();

            try {
                _validate.check(Request, new string[] {"medico"});

                apadrinado = apadrinado.get(cod, _context);
                return Ok(apadrinado);
            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return NotFound();
            }
        }


        [HttpGet("apadrinado/foto/{cod}")]
        [ResponseCache(Duration = 60 * 60 * 48, Location = ResponseCacheLocation.Client)] //cache de 60 * 60 * 48 segundos = 48 horas
        public IActionResult GetApadrinadoFoto(int cod)
        {
            string query = $"EXEC dbo.select_Apadrinado_foto @cod={cod}";
            Foto foto;
            try {
                _validate.check(Request, new string[] {"medico" });

                foto = _context.Fotos.FromSql(query).First();
                var image = System.IO.File.OpenRead($"\\\\Guysrv08\\aptifyphoto\\DPHOTO\\Images\\{foto.path}\\{foto.name}");
                return File(image, "image/jpeg");
            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return Redirect("/images/ci.png");
            }
        }
    }
}
