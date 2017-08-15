﻿using Microsoft.AspNetCore.Mvc;

namespace TheCircle.Controllers.views
{
    public class TheCircle : Controller
    {

        [HttpGet ("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet ("asistente")]
        public IActionResult AsistenteSalud()
        {
            return View();
        }

        [HttpGet ("medico")]
        public IActionResult Medico()
        {
            return View();
        }

        [HttpGet("coordinador")]
        public IActionResult CoordinadorSalud()
        {
            return View();
        }

        [HttpGet("contralor/")]
        public IActionResult Contralor()
        {
            return View();
        }
    }
}
