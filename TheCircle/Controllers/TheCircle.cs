using Microsoft.AspNetCore.Mvc;
using System;
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
        [VIEWauth("asistente")]
        public IActionResult AsistenteSalud()
        {
            return View();
        }

        [HttpGet ("medico")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [VIEWauth("medico")]
        public IActionResult Medico()
        {
            return View();
        }

        [HttpGet("coordinador")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [VIEWauth("coordinador")]
        public IActionResult CoordinadorSalud()
        {
            return View();
        }

        [HttpGet("contralor")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [VIEWauth("contralor")]
        public IActionResult Contralor()
        {
            return View();
        }

        [HttpGet("bodeguero")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [VIEWauth("bodeguero")]
        public IActionResult Bodeguero()
        {
            return View();
        }

        [HttpGet("sistema")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [VIEWauth("sistema")]
        public IActionResult Sistema()
        {         
            return View();
        }

        [HttpGet("coordinadorcc")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [VIEWauth("cordinadorCC")]
        public IActionResult CoordinadorCC()
        {
            return View();
        }
    }
}
