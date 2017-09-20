using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TheCircle.Models;
using TheCircle.Util;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    public class LoginController : Controller
    {

        private readonly MyDbContext _context;
        public LoginController(MyDbContext context)
        {
            _context = context;
        }


        [HttpGet("logout")]
        public IActionResult Logout([FromQuery] LoginMessage lm)
        {
            foreach (var cookie in Request.Cookies.Keys)
                Response.Cookies.Delete(cookie);

            if (ModelState.IsValid) {
                var parameters = new Dictionary<string, string> { { "flag", $"{lm.flag}" }, { "msg", lm.msg } };
                var loginRedirect = QueryHelpers.AddQueryString("/", parameters);
                return Redirect(loginRedirect);
            }

            return Redirect("/");
        }

        [HttpPost("login")]
        public IActionResult login([FromForm] LoginRequest request)
        {

            Dictionary<string, string> parameters;
            string loginRedirect;


            if (!ModelState.IsValid)
            {
                parameters = new Dictionary<string, string> { { "flag", "21" }, { "msg", "Precaucion, data fuera de rangos" } };
                loginRedirect = QueryHelpers.AddQueryString("/", parameters);
                return Redirect(loginRedirect);
            }


            try
            {
                Usuario usuario = Usuario.Get(request);
                Token token = new Token(usuario, request.localidad);
                string token_string = JsonConvert.SerializeObject(token);

                var options = new CookieOptions()
                {
                    Expires = token.data.expireAt,
                    HttpOnly = true
                };

                var publicOptions = new CookieOptions()
                {
                    Expires = token.data.expireAt,
                    HttpOnly = false
                };

                Response.Cookies.Append("session", Signature.ToBase(token_string), options);
                Response.Cookies.Append("session_name", $"{token.data.nombres} {token.data.apellidos}", publicOptions);
                Response.Cookies.Append("session_email", token.data.email, publicOptions);
                Response.Cookies.Append("session_photo", $"/api/user/{token.data.cedula}/photo", publicOptions);


                if (Token.CheckLocalidad(token) != null)
                    return Token.CheckLocalidad(token);

                switch (token.data.cargo) {
                    case "medico":
                        return Redirect("/medico");
                    case "asistenteSalud":
                        return Redirect("/asistente");
                    case "sistema":
                        return Redirect("/sistema");
                    case "bodeguero":
                        return Redirect("/bodeguero");
                    case "coordinador":
                        return Redirect("/coordiandor");
                    case "contralor":
                        return Redirect("/contralor");
                    case "coordinadorCC":
                        return Redirect("/coordinadorcc");
                    default:
                        return Redirect("logout");
                }

            } catch (Exception e) { //Si el usuario es invalido o se evidencia algun error
                parameters = new Dictionary<string, string> { { "flag", "21" }, { "msg", "Usuario/Clave incorrecto" } };
                loginRedirect = QueryHelpers.AddQueryString("/", parameters);
                return Redirect(loginRedirect);
            }
        }

        [HttpGet("login")]
        [APIauth("medico", "asistenteSalud", "sistema", "bodeguero", "coordinador", "contralor", "coordinadorCC")]
        public IActionResult loginCheck(Token token)
        {
            return Ok(token);
        }
    }
}