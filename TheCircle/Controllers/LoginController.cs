using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using TheCircle.Models;

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
            //KeyValuePair<string, string>[] cookies = Request.Cookies.ToArray(); //Todas las cookies

            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(-5); //Ya han expirado
            options.HttpOnly = true; //no js}
            //options.Path = "/medico";
            //foreach (KeyValuePair<string, string> cookie in cookies)
            //{
            Response.Cookies.Append("session", "", options); //Se borra la data
                //Request.Cookies[cookie.Value] =
                //cookie.Value = DateTime.Now.AddDays(-1);
            //}
            return Redirect("/");
        }

        [HttpPost("login")]
        public IActionResult login([FromForm] LoginRequest request) {

            User usuario = new User();
            Url login = new Url("/");

            if (ModelState.IsValid) {

                try {
                    usuario = usuario.get(request, _context);

                    Token token = new Token(usuario, request.localidad);

                    //Data data = new Data(usuario, request);
                    //Token token = new Token(data, "asdasdx324c23rx4vtewq4tvgr4b54vuw6uwj6j465cf");
                    string tokenToString = JsonConvert.SerializeObject(token);

                    CookieOptions options = new CookieOptions();
                    options.Expires = data.expireAt;
                    options.HttpOnly = true;
                    Response.Cookies.Append("session", tokenToString, options);

                    if (data.cargo == "medico")
                    {
                        return Redirect("/medico");
                    }
                    else if (data.cargo == "asistenteSalud")
                    {
                        return Redirect("/asistente");
                    }
                    else {
                        return Redirect("logout");
                    }

                } catch (Exception e) {
                    login.SetQueryParam("success", "0");
                    login.SetQueryParam("msg", "Usuario/Clave incorrecto");
                    return Redirect(login);
                    //return Redirect("/?validate=1");
                }
            }
            login.SetQueryParam("success", "0");
            login.SetQueryParam("msg", "Usuario/Clave incorrecto");
            return Redirect(login);
            //return BadRequest("Incorrect data");
        }
    }
}
