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
            foreach (var cookie in Request.Cookies.Keys)
                Response.Cookies.Delete(cookie);

            return Redirect("/login");
        }

        [HttpGet("login")]
        //[ResponseCache(Duration = 60 * 60 * 120, Location = ResponseCacheLocation.Client)] //cache de 60*60*60 segundos = 120 horas
        public IActionResult Login()
        {
            return View("~/Views/TheCircle/Login.cshtml");
        }

        [HttpPost("login")]
        public IActionResult login([FromForm] LoginRequest req)
        {
            if (ModelState.IsValid is false)
                return BadRequest("Usuario/Clave incorrectos");

            try
            {
                Usuario usuario = Usuario.Get(req); //Se obtiene el usuario de la BDD
                Token token = new Token(usuario, req.localidad); //Se genera el token despues de validar credenciales
                Token.CheckLocalidad(token.data); //Se comprueba la localidad segun su cargo

                string token_string = JsonConvert.SerializeObject(token);                

                var options = new CookieOptions() {
                    Expires = token.data.expireAt,
                    HttpOnly = true
                };

                Response.Cookies.Append("session", Signature.ToBase(token_string), options);

                return Ok();

            } catch (Exception e) {
                if (e is LocalidadException)
                    return BadRequest("localidad incorrecta, por favor verifique su lugar de sesión");
                else if (e is UserInactivoException)
                    return BadRequest("Usuario inactivo, por favor contacte a sistemas");
                else
                    return BadRequest("Usuario/Clave incorrectos");                
            }
        }

        [HttpGet("session")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult loginCheck()
        {
            try {
                string cookie = Request.Cookies["session"];
                Token token = JsonConvert.DeserializeObject<Token>(Signature.FromBase(cookie));
                Token.CheckValid(token);

                return Ok();
            } catch (Exception e) {
                return Unauthorized();
            }           
        }
    }
}