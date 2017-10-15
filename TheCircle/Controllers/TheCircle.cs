using Microsoft.AspNetCore.Mvc;
using TheCircle.Util;
using TheCircle.Models;

namespace TheCircle.Controllers.views
{
    public class TheCircle : Controller
    {

        public TheCircle() { }

        //Ruta principal de los apps TheCircle
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
    }
}
