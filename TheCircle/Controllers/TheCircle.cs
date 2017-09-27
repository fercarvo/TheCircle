using Microsoft.AspNetCore.Mvc;
using TheCircle.Util;

namespace TheCircle.Controllers.views
{
    public class TheCircle : Controller
    {

        public TheCircle() { }

        [HttpGet("")]
        [VIEWauth()]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(Token token)
        {
            ViewData["name"] = $"{token.data.nombres} {token.data.apellidos}";
            ViewData["email"] = token.data.email;
            ViewData["photo"] = $"/api/user/{token.data.cedula}/photo";

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
        public IActionResult AsistenteSalud()
        {
            return View();
        }

        [HttpGet ("medico")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Medico()
        {
            return View();
        }

        [HttpGet("coordinador")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult CoordinadorSalud()
        {
            return View();
        }

        [HttpGet("contralor")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Contralor()
        {
            return View();
        }

        [HttpGet("bodeguero")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Bodeguero()
        {
            return View();
        }

        [HttpGet("sistema")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Sistema()
        {         
            return View();
        }

        [HttpGet("coordinadorcc")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
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

        [HttpGet("imprimir")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Imprimir()
        {
            return View();
        }
    }
}
