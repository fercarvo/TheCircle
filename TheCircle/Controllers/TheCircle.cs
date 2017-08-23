using Microsoft.AspNetCore.Mvc;

namespace TheCircle.Controllers.views
{
    public class TheCircle : Controller
    {

        [HttpGet ("")]
        [ResponseCache(Duration = 60*60*120, Location = ResponseCacheLocation.Client)] //cache de 60*60*60 segundos = 120 horas
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet ("asistente")]
        [ResponseCache(Duration = 60 * 60 * 120, Location = ResponseCacheLocation.Client)] //cache de 60*60*60 segundos = 120 horas
        public IActionResult AsistenteSalud()
        {
            return View();
        }

        [HttpGet ("medico")]
        [ResponseCache(Duration = 60 * 60 * 120, Location = ResponseCacheLocation.Client)] //cache de 60*60*60 segundos = 120 horas
        public IActionResult Medico()
        {
            if (true) //validaciones
            {
                return View();
            }
        }

        [HttpGet("coordinador")]
        [ResponseCache(Duration = 60 * 60 * 120, Location = ResponseCacheLocation.Client)] //cache de 60*60*60 segundos = 120 horas
        public IActionResult CoordinadorSalud()
        {
            return View();
        }

        [HttpGet("contralor/")]
        [ResponseCache(Duration = 60 * 60 * 120, Location = ResponseCacheLocation.Client)] //cache de 60*60*60 segundos = 120 horas
        public IActionResult Contralor()
        {
            return View();
        }

        [HttpGet("bodeguero/")]
        [ResponseCache(Duration = 60 * 60 * 120, Location = ResponseCacheLocation.Client)] //cache de 60*60*60 segundos = 120 horas
        public IActionResult Bodeguero()
        {
            return View();
        }
    }
}
