﻿using Microsoft.AspNetCore.Mvc;
using TheCircle.Util;

namespace TheCircle.Controllers.views
{
    public class TheCircle : Controller
    {

        public TheCircle() { }

        [HttpGet("login")]
        [ResponseCache(Duration = 60*60*120, Location = ResponseCacheLocation.Client)] //cache de 60*60*60 segundos = 120 horas
        public IActionResult Login([FromQuery] Message query)
        {
            if (ModelState.IsValid)
                ViewData["mensaje"] = query.msg;

            return View("Login");
        }

        [HttpGet("")]
        [VIEWauth()]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(Token token)
        {
            switch (token.data.cargo)
            {
                case "medico":
                    return View("Medico");
                case "asistenteSalud":
                    return View("AsistenteSalud");
                case "sistema":
                    return View("Sistema");
                case "bodeguero":
                    return View("Bodeguero");
                case "coordinador":
                    return View("CoordinadorSalud");
                case "contralor":
                    return View("Contralor");
                case "coordinadorCC":
                    return View("CoordinadorCC");
                case "director":
                    return View("Director");
                default:
                    return LocalRedirect("/login");
            }
        }

        [HttpGet ("asistente")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [VIEWauth("asistenteSalud")]
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
        [VIEWauth("coordinadorCC")]
        public IActionResult CoordinadorCC()
        {
            return View();
        }

        [HttpGet("director")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Director()
        {
            return View();
        }
    }
}
