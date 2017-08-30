using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using System;
using TheCircle.Util;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class UserController : Controller
    {
        private readonly MyDbContext _context;
        private Token _validate;

        public UserController (MyDbContext context)
        {
            _context = context;
            _validate = new Token();
        }

        [HttpGet("user")]
        [ResponseCache(Duration = 60*10, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_All_Users()
        {
            var u = new UserSafe();

            try {

                _validate.check(Request, new string[] { "sistema" });

                var usuarios = u.getAll(_context);
                return Ok(usuarios);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return NotFound();
            }
            
        }

        [HttpGet("user/activos")]
        [ResponseCache(Duration = 1, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_All_Users_Activos()
        {
            var u = new UserSafe();

            try {

                _validate.check(Request, new string[] { "sistema" });

                var usuarios = u.getActivos(_context);
                return Ok(usuarios);
                
            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return NotFound();
            }            
        }

        [HttpGet("user/inactivos")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_All_Users_Inactivos()
        {
            var u = new UserSafe();

            try {

                _validate.check(Request, new string[] {"sistema"});

                var usuarios = u.getInactivos(_context);
                return Ok(usuarios);
                
            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return NotFound();
            }            
        }


        [HttpPut("user/{id}/activar")]
        public IActionResult User_Activar(int id) 
        {
            var u = new User();

            if (id <= 0)
                return BadRequest("Incorrect Data");

            try {

                _validate.check(Request, new string[] {"sistema"});

                u.activar(id, _context);
                return Ok();

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }            
        }

        [HttpPut("user/{id}/desactivar")]
        public IActionResult User_Desactivar(int id) 
        {
            var u = new User();

            if (id <= 0)
                return BadRequest("Incorrect Data");

            try {

                _validate.check(Request, new string[] {"sistema"});

                u.desactivar(id, _context);
                return Ok();

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }            
        }

        [HttpPut("user/{id}/clave/set")]
        public IActionResult User_SetClave(int id) 
        {
            var u = new User();

            if (id <= 0)
                return BadRequest("Incorrect Data");

            try {

                _validate.check(Request, new string[] {"sistema"});

                string clave = u.nueva_clave(id, _context);
                return Ok( new {clave = clave} );

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }            
        }

        [HttpPut("user/clave")]
        public IActionResult User_CambiarClave([FromForm] Clave req ) 
        {
            var u = new User();

            if (req == null)
                return BadRequest("Incorrect Data");

            try {

                //var token = _validate.check(Request, new string[] {"sistema, medico, asistenteSalud, bodeguero"});

                u.cambiar_clave(req.cedula, req.actual, req.nueva, _context);
                var parameters = new Dictionary<string, string> { { "flag", "21" }, { "msg", "Contraseña cambiada con exito" } };
                var loginRedirect = QueryHelpers.AddQueryString("/", parameters);
                return Redirect(loginRedirect);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }            
        }
    }
}