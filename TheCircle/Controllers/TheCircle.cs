using Microsoft.AspNetCore.Mvc;

namespace TheCircle.Controllers.views
{
    public class TheCircle : Controller
    {
        // GET: /TheCircle/
        // GET: /
        //[HttpGet ("medico")]
        public IActionResult Index()
        {
            return View();
        }

        // GET: /TheCircle/AsistenteSalud
        // GET: /AsistenteSalud
        //[HttpGet ("asistente")]
        public IActionResult AsistenteSalud()
        {
            return View();
        }

        //[HttpGet ("asistente")]
        public IActionResult Medico()
        {
            return View();
        }
    }
}
