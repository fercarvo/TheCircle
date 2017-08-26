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

                    parameters = new Dictionary<string, string> { { "success", "0" }, { "msg", "Usuario/Clave incorrecto" } };
                    loginRedirect = QueryHelpers.AddQueryString("/", parameters);

                    return Redirect(loginRedirect);
                }
            } //Si la data enviada en el formulario esta incorrecta

            parameters = new Dictionary<string, string> { { "success", "0" }, { "msg", "Precaucion, data fuera de rangos" } };
            loginRedirect = QueryHelpers.AddQueryString("/", parameters);

            return Redirect(loginRedirect);
        }
    }
}
