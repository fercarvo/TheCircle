using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using System;
using TheCircle.Util;

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
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
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
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_All_Users()
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
        public IActionResult Get_All_Users()
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
        public IActionResult PostDespachoReceta(int id) 
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
        public IActionResult PostDespachoReceta(int id) 
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
        public IActionResult PostDespachoReceta(int id) 
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
        public IActionResult PostDespachoReceta([FromBody] Clave req ) 
        {
            var u = new User();

            if (req == null)
                return BadRequest("Incorrect Data");

            try {

                var token = _validate.check(Request, new string[] {"sistema, medico, asistenteSalud, bodeguero"});

                u.cambiar_clave(token.data.cedula, clave.actual, clave.nueva, _context);
                return Ok();

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }            
        }
    }
}