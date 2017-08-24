using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TheCircle.Models;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    public class LoginController : Controller
    {

        [HttpGet("logout")]
        public IActionResult Logout([FromHeader]Data data)
        {

            return Redirect("/");
        }

        [HttpPost("login")]
        [blablabla]
        public IActionResult login([FromBody] LoginRequest req)
        {
            if (ModelState.IsValid)
            {
                Data data = new Data();

                data.path = "/medico";
                data.cedula = req.cedula;
                data.nombres = "Edgar Fernando";
                data.apellidos = "Carvajal Ulloa";
                data.cargo = "medico";
                data.localidad = "CC2";
                data.path = "/medico";
                data.issueAt = DateTime.Now;
                data.expireAt = DateTime.Now.AddHours(24);

                string sign = "asdasdx324c23rx4vtewq4tvgr4b54vuw6uwj6j465cf";

                Token token = new Token(data, sign);

                string t = token.ToString();

                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(1);

                Response.Cookies.Append("token", t, options);
                Response.Headers.Append("token", t);
                
                return Ok(token);
            }
            return BadRequest("Incorrect data");
        }
    }
}
