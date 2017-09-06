using Microsoft.AspNetCore.Mvc;
using TheCircle.Util;

namespace TheCircle.Controllers.views
{
    public class TheCircle : Controller
    {

        public TheCircle() { }

        [HttpGet ("")]
        [ResponseCache(Duration = 60*60*120, Location = ResponseCacheLocation.Client)] //cache de 60*60*60 segundos = 120 horas
        public ActionResult Index([FromQuery] LoginMessage lm)
        {
            if (ModelState.IsValid)
                if (lm.flag == 21)
                    ViewData["mensaje"] = lm.msg;
            return View();
        }

        [HttpGet ("asistente")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [Allow("asistente")]
        public IActionResult AsistenteSalud(Token token)
        {
            if (token is null)
                return Redirect("/");

            return View();
        }

        [HttpGet ("medico")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [Allow("medico")]
        public IActionResult Medico(Token token)
        {
            if (token is null)
                return Redirect("/");

            return View();
        }

        [HttpGet("coordinador")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [Allow("coordinador")]
        public IActionResult CoordinadorSalud(Token token)
        {
            if (token is null)
                return Redirect("/");

            return View();
        }

        [HttpGet("contralor")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [Allow("contralor")]
        public IActionResult Contralor(Token token)
        {
            if (token is null)
                return Redirect("/");

            return View();
        }

        [HttpGet("bodeguero")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [Allow("bodeguero")]
        public IActionResult Bodeguero(Token token)
        {
            if (token is null)
                return Redirect("/");

            return View();
        }

        [HttpGet("sistema")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [Allow("sistema")]
        public IActionResult Sistema(Token token)
        {
            if (token is null)
                return Redirect("/");

            return View();
        }
    }
}
