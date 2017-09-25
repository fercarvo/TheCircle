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
        public IActionResult Logout([FromQuery] Message query)
        {
            foreach (var cookie in Request.Cookies.Keys)
                Response.Cookies.Delete(cookie);

            if (ModelState.IsValid) {
                var parameters = new Dictionary<string, string> {{ "msg", query.msg }};
                var url = QueryHelpers.AddQueryString("/login", parameters);
                return Redirect(url);
            }

            return Redirect("/login");
        }

        [HttpPost("login")]
        public IActionResult login([FromForm] LoginRequest req)
        {
            if (ModelState.IsValid is false)
                return CredentialsRedirect();

            try
            {
                Usuario usuario = Usuario.Get(req); //Se obtiene el usuario de la BDD
                Token token = new Token(usuario, req.localidad); //Se genera el token despues de validar credenciales
                Token.CheckLocalidad(token.data); //Se comprueba la localidad segun su cargo

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

                //return CargoRedirect(token);
                return LocalRedirect("/");

            } catch (Exception e) {
                if (e is LocalidadException)
                    return LocalidadRedirect();
                if (e is TokenException)
                    return CredentialsRedirect();

                return CredentialsRedirect();                
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

        public RedirectResult LocalidadRedirect(){
            var parameters = new Dictionary<string, string> {{ "msg", "Localidad incorrecta" }};
            var url = QueryHelpers.AddQueryString("/login", parameters);
            return new RedirectResult(url);            
        }

        public RedirectResult CredentialsRedirect(){
            var parameters = new Dictionary<string, string> {{ "msg", "Usuario/Clave incorrecto" } };
            var url = QueryHelpers.AddQueryString("/login", parameters);
            return new RedirectResult(url);        
        }
    }
}