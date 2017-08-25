using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TheCircle.Models;

namespace TheCircle.Controllers.views
{
    public class TheCircle : Controller
    {

        [HttpGet ("")]
        //[ResponseCache(Duration = 60*60*120, Location = ResponseCacheLocation.Client)] //cache de 60*60*60 segundos = 120 horas
        public ActionResult Index([FromQuery]int validate)
        {
            if (validate == 1) {
                ViewData["login"] = "Usuario/Clave incorrectos";
            }
            
            //var data = TempData["msg"];
            return View();
        }

        [HttpGet ("asistente")]
        //[ResponseCache(Duration = 60 * 60 * 120, Location = ResponseCacheLocation.Client)] //cache de 60*60*60 segundos = 120 horas
        public IActionResult AsistenteSalud()
        {
            return View();
        }

        [HttpGet ("medico")]
        //[ResponseCache(Duration = 60 * 60 * 120, Location = ResponseCacheLocation.Client)] //cache de 60*60*60 segundos = 120 horas
        public IActionResult Medico()
        {
            var cookieSession = Request.Cookies["session"];
            //var tokenToString = token.ToString();
            Token tokenReversed = JsonConvert.DeserializeObject<Token>(cookieSession);

            //Response.Headers.Append("token", t);

            //var font = Request.Cookies["session"];
            //return Ok(new {token1 = token, token2 = t});

            if (string.IsNullOrEmpty(cookieSession)) 
            {
                return Redirect("/");
            }
            else
            {
                return View();
            }
        }

        [HttpGet("coordinador")]
        //[ResponseCache(Duration = 60 * 60 * 120, Location = ResponseCacheLocation.Client)] //cache de 60*60*60 segundos = 120 horas
        public IActionResult CoordinadorSalud()
        {
            return View();
        }

        [HttpGet("contralor/")]
        //[ResponseCache(Duration = 60 * 60 * 120, Location = ResponseCacheLocation.Client)] //cache de 60*60*60 segundos = 120 horas
        public IActionResult Contralor()
        {
            return View();
        }

        [HttpGet("bodeguero/")]
        //[ResponseCache(Duration = 60 * 60 * 120, Location = ResponseCacheLocation.Client)] //cache de 60*60*60 segundos = 120 horas
        public IActionResult Bodeguero()
        {
            return View();
        }
    }
}
