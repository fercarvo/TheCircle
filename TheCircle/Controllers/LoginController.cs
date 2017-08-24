using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    public class LoginController : Controller
    {

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            return Redirect("/");
        }

        [HttpPost("login")]
        public IActionResult login([FromBody] LoginRequest req)
        {
            if (ModelState.IsValid)
            {
                return Redirect("/medico");
            }
            return Redirect("/asistente");
        }
    }
}
