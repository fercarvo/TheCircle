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
            try {

                _validate.check(Request, new string[] { "sistema" });

                UserSafe[] usuarios = new UserSafe().getAll(_context);
                return Ok(usuarios);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
            
        }

        [HttpGet("user/activos")]
        [ResponseCache(Duration = 1, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_All_Users_Activos()
        {
            try {

                _validate.check(Request, new string[] { "sistema" });

                UserSafe[] usuarios = new UserSafe().getActivos(_context);
                return Ok(usuarios);
                
            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }            
        }

        [HttpGet("user/inactivos")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_All_Users_Inactivos()
        {
            try {

                _validate.check(Request, new string[] {"sistema"});

                UserSafe[] usuarios = new UserSafe().getInactivos(_context);
                return Ok(usuarios);
                
            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }            
        }

        [HttpPost("user")]
        public IActionResult Login_create([FromForm]string cedula, [FromForm]string clave)
        {
            if (string.IsNullOrEmpty(cedula) || string.IsNullOrEmpty(clave))
                return BadRequest("Datos incorrectos");

            try {
                new User().crear(cedula, clave, _context);
                return Ok();

            } catch (Exception e) {
                return BadRequest("Error al crear usuario");
            }
        }


        [HttpPut("user/{id}/activar")]
        public IActionResult User_Activar(int id) 
        {
            if (id <= 0)
                return BadRequest("Incorrect Data");

            try {

                _validate.check(Request, new string[] {"sistema"});

                new User().activar(id, _context);
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
            if (id <= 0)
                return BadRequest("Incorrect Data");

            try {

                _validate.check(Request, new string[] {"sistema"});

                new User().desactivar(id, _context);
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
            if (id <= 0)
                return BadRequest("Incorrect Data");

            try {

                _validate.check(Request, new string[] {"sistema"});

                string clave = new User().nueva_clave(id, _context);
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
            if (req == null)
                return BadRequest("Datos incorrectos");

            try {
                new User().cambiar_clave(req.cedula, req.actual, req.nueva, _context);
                return Ok();

            } catch (Exception e) {
                return BadRequest("No se pudo cambiar la contraseña");
            }            
        }
    }
}