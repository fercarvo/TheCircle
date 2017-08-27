using Microsoft.AspNetCore.Mvc;
using System;
using TheCircle.Util;

namespace TheCircle.Controllers.views
{
    public class TheCircle : Controller
    {

        private readonly Token _validate;
        public TheCircle()
        {
            _validate = new Token();
        }

        [HttpGet ("")]
        [ResponseCache(Duration = 60*60*120, Location = ResponseCacheLocation.Client)] //cache de 60*60*60 segundos = 120 horas
        public ActionResult Index([FromQuery] int flag, [FromQuery] string msg)
        {
            if (flag == 0)
                ViewData["mensaje"] = msg;
            return View();
        }

        [HttpGet ("asistente")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult AsistenteSalud()
        {
            try {
                //_validate.check(Request, "asistenteSalud");
                return View();
            } catch (Exception e) {
                return Redirect("/");
            }
        }

        [HttpGet ("medico")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Medico()
        {
            try {
                //_validate.check(Request, "medico");
                return View();
            } catch (Exception e) {
                return Redirect("/");
            }
        }

        [HttpGet("coordinador")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult CoordinadorSalud()
        {
            return View();
        }

        [HttpGet("contralor/")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Contralor()
        {
            return View();
        }

        [HttpGet("bodeguero/")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)] //cache de 60*60*60 segundos = 120 horas
        public IActionResult Bodeguero()
        {
            return View();
        }
    }
}
