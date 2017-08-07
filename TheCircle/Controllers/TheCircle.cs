using Microsoft.AspNetCore.Mvc;

namespace TheCircle.Controllers.views
{
    public class TheCircle : Controller
    {
        // GET: /TheCircle/
        // GET: /
        [HttpGet ("")]
        public IActionResult Index()
        {
            return View();
        }

        // GET: /TheCircle/AsistenteSalud
        // GET: /AsistenteSalud
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
    }
}
