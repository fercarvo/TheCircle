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
            CookieOptions options = new CookieOptions() {
                Expires = DateTime.Now.AddDays(-5), 
                HttpOnly = true
            };

            Response.Cookies.Append("session", "", options);
            Response.Cookies.Append("session_nombre", "", options);

            if (ModelState.IsValid) {
                var parameters = new Dictionary<string, string> { { "flag", $"{lm.flag}" }, { "msg", lm.msg } };
                var loginRedirect = QueryHelpers.AddQueryString("/", parameters);
                return Redirect(loginRedirect);    
            }

            return Redirect("/");
        }

        [HttpPost("login")]
        public IActionResult login([FromForm] LoginRequest request) {

            User usuario = new User();
            Dictionary<string, string> parameters;
            string loginRedirect;


            if (!ModelState.IsValid) {
                parameters = new Dictionary<string, string> { { "flag", "21" }, { "msg", "Precaucion, data fuera de rangos" } };
                loginRedirect = QueryHelpers.AddQueryString("/", parameters);
                return Redirect(loginRedirect);    
            }


            try 
            {
                usuario = usuario.get(request, _context);
                Token token = new Token(usuario, request.localidad);
                string token_string = JsonConvert.SerializeObject(token);

                var options = new CookieOptions() {
                    Expires = token.data.expireAt,
                    HttpOnly = true
                };

                var optionsNombre = new CookieOptions() {
                    Expires = token.data.expireAt,
                    HttpOnly = false
                };

                Response.Cookies.Append("session", new Signature().toBase(token_string), options);
                Response.Cookies.Append("session_nombre", $"{token.data.nombres} {token.data.apellidos}", optionsNombre);


                if (token.data.cargo == "medico")
                        return Redirect("/medico");

                    if (token.data.cargo == "asistenteSalud")
                        return Redirect("/asistente");

                    if (token.data.cargo == "sistema")
                        return Redirect("/sistema");

                    if (token.data.cargo == "bodeguero")
                        return Redirect("/bodeguero");

                    if (token.data.cargo == "coordinador")
                        return Redirect("/coordiandor");

                    if (token.data.cargo == "contralor")
                        return Redirect("/contralor");

                    if (token.data.cargo == "coordinadorCC")
                        return Redirect("/coordinadorCC");

                return Redirect("logout");

            } catch (Exception e) { //Si el usuario es invalido o se evidencia algun error
                parameters = new Dictionary<string, string> { { "flag", "21" }, { "msg", "Usuario/Clave incorrecto" } };
                loginRedirect = QueryHelpers.AddQueryString("/", parameters);
                return Redirect(loginRedirect);
            }            
        }
    }
}
