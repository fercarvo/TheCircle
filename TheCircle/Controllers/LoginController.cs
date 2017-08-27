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
        public IActionResult Logout()
        {
            CookieOptions options = new CookieOptions() {
                Expires = DateTime.Now.AddDays(-5), //Ya han expirado
                HttpOnly = true
            };

            Response.Cookies.Append("session", "", options); //Se borra la data
            return Redirect("/");
        }

        [HttpPost("login")]
        public IActionResult login([FromForm] LoginRequest request) {

            User usuario = new User();
            Dictionary<string, string> parameters;
            string loginRedirect;

            if (ModelState.IsValid) {

                try {
                    usuario = usuario.get(request, _context);
                    Token token = new Token(usuario, request.localidad);
                    string tokenToString = JsonConvert.SerializeObject(token);

                    var options = new CookieOptions() {
                        Expires = token.data.expireAt,
                        HttpOnly = true
                    };

                    Response.Cookies.Append("session", tokenToString, options);

                    if (token.data.cargo == "medico")
                        return Redirect("/medico");
                    else if (token.data.cargo == "asistenteSalud")
                        return Redirect("/asistente");
                    else
                        return Redirect("logout");

                } catch (Exception e) { //Si el usuario es invalido o se evidencia algun error

                    parameters = new Dictionary<string, string> { { "flag", "21" }, { "msg", "Usuario/Clave incorrecto" } };
                    loginRedirect = QueryHelpers.AddQueryString("/", parameters);

                    return Redirect(loginRedirect);
                }
            } //Si la data enviada en el formulario esta incorrecta

            parameters = new Dictionary<string, string> { { "flag", "21" }, { "msg", "Precaucion, data fuera de rangos" } };
            loginRedirect = QueryHelpers.AddQueryString("/", parameters);

            return Redirect(loginRedirect);
        }

        [HttpPost("login/reset")]
        public IActionResult Login_reset([FromForm]int cedula, [FromForm]string email)
        {
            if ( cedula<=0 || string.IsNullOrEmpty(email))
                return Redirect("/");

            try {
                var user = new User();
                user.reset_clave(cedula, email, _context);
                var parameters = new Dictionary<string, string> { { "flag", "21" }, { "msg", "Reseteo de clave exitoso, verifique su email institucional" } };
                var loginRedirect = QueryHelpers.AddQueryString("/", parameters);

                return Redirect(loginRedirect);

            } catch (Exception e) {
                var parameters = new Dictionary<string, string> { { "flag", "21" }, { "msg", "No se ha podido ejecutar el reseteo de clave, favor verifique datos o contactese con Gerencia de sistemas" } };
                var loginRedirect = QueryHelpers.AddQueryString("/", parameters);
                return Redirect(loginRedirect);

                return Redirect(loginRedirect);
            }
        }

        [HttpPost("login/create")]
        public IActionResult Login_create([FromForm]int cedula, [FromForm]string clave)
        {
            if ( cedula<=0 || string.IsNullOrEmpty(clave))
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

                return Redirect(loginRedirect);
            }
        }
    }
}
