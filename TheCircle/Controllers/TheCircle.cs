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
        public ActionResult Index([FromQuery] LoginMessage lm)
        {
            if (ModelState.IsValid)
                if (lm.flag == 21)
                    ViewData["mensaje"] = lm.msg;
            return View();
        }

        [HttpGet ("asistente")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult AsistenteSalud()
        {
            try {
                _validate.check(Request, new string[] { "asistenteSalud" });
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
                _validate.check(Request, new string[] { "medico" });
                return View();
            } catch (Exception e) {
                return Redirect("/");
            }
        }

        [HttpGet("coordinador")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult CoordinadorSalud()
        {
            try
            {
                _validate.check(Request, new string[] { "coordinador" });
                return View();
            }
            catch (Exception e)
            {
                return Redirect("/");
            }
        }

        [HttpGet("contralor/")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Contralor()
        {
            try
            {
                _validate.check(Request, new string[] { "contralor" });
                return View();
            }
            catch (Exception e)
            {
                return Redirect("/");
            }
        }

        [HttpGet("bodeguero/")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Bodeguero()
        {
            try
            {
                _validate.check(Request, new string[] { "bodeguero" });
                return View();
            }
            catch (Exception e)
            {
                return Redirect("/");
            }
        }

        [HttpGet("sistema/")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Sistema()
        {
            try
            {
                _validate.check(Request, new string[] { "sistema" });
                return View();
            }
            catch (Exception e)
            {
                return Redirect("/");
            }
        }
    }
}
