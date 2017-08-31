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
            _signer = new Signature();
        }


        [HttpGet("logout")]
        public IActionResult Logout([FromQuery] LoginMessage lm)
        {
            CookieOptions options = new CookieOptions() {
                Expires = DateTime.Now.AddDays(-5), 
                HttpOnly = true
            };

            Response.Cookies.Append("session", "", options);

            if (lm != null) {
                var parameters = new Dictionary<string, string> { { "flag", lm.flag }, { "msg", lm.msg } };
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

                Response.Cookies.Append("session", token_string, options);
                //Response.Cookies.Append("session", _signer.toBase(token_string), options);

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
                        
                    return Redirect("logout");

            } catch (Exception e) { //Si el usuario es invalido o se evidencia algun error
                parameters = new Dictionary<string, string> { { "flag", "21" }, { "msg", "Usuario/Clave incorrecto" } };
                loginRedirect = QueryHelpers.AddQueryString("/", parameters);
                return Redirect(loginRedirect);
            }
            
        }


        [HttpPost("login/crear")]
        public IActionResult Login_create([FromForm]string cedula, [FromForm]string clave)
        {
            if (string.IsNullOrEmpty(cedula) || string.IsNullOrEmpty(clave))
                return Redirect("/");

            try {
                var user = new User();
                user.crear(cedula, clave, _context);
                var parameters = new Dictionary<string, string> { { "flag", "21" }, { "msg", "Usuario creado exitosamente, a continuación sistemas debera validar sus datos, este atento de su email." } };
                var loginRedirect = QueryHelpers.AddQueryString("/", parameters);

                return Redirect(loginRedirect);

            } catch (Exception e) {
                var parameters = new Dictionary<string, string> { { "flag", "21" }, { "msg", "No se ha podido crear su usuario, datos incorrectos o usuario ya existente, si el problema persiste consulte a sistemas." } };
                var loginRedirect = QueryHelpers.AddQueryString("/", parameters);
                return Redirect(loginRedirect);
            }
        }
    }
}
