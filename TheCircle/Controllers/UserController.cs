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
        public UserController (MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("user")]
        [ResponseCache(Duration = 60*10, Location = ResponseCacheLocation.Client)]
        [Allow("sistema")]
        public IActionResult Get_All_Users(Token token)
        {
            if (token is null)
                return Unauthorized();

            try
            {
                UserSafe[] usuarios = new UserSafe().getAll(_context);
                return Ok(usuarios);

            } catch (Exception e) {
                return BadRequest("Something broke");
            }
            
        }

        [HttpGet("user/activos")]
        [Allow("sistema")]
        [ResponseCache(Duration = 1, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_All_Users_Activos(Token token)
        {
            if (token is null)
                return Unauthorized();

            try
            {
                UserSafe[] usuarios = new UserSafe().getActivos(_context);
                return Ok(usuarios);
                
            } catch (Exception e) {
                return BadRequest("Something broke");
            }            
        }

        [HttpGet("user/inactivos")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        [Allow("sistema")]
        public IActionResult Get_All_Users_Inactivos(Token token)
        {
            if (token is null)
                return Unauthorized();

            try
            {
                UserSafe[] usuarios = new UserSafe().getInactivos(_context);
                return Ok(usuarios);
                
            } catch (Exception e) {
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
        [Allow("sistema")]
        public IActionResult User_Activar(Token token, int id) 
        {
            if (id <= 0)
                return BadRequest("Incorrect Data");
            if (token is null)
                return Unauthorized();

            try
            {
                new User().activar(id, _context);
                return Ok();

            } catch (Exception e) {
                return BadRequest("Something broke");
            }            
        }

        [HttpPut("user/{id}/desactivar")]
        [Allow("sistema")]
        public IActionResult User_Desactivar(Token token, int id) 
        {
            if (id <= 0)
                return BadRequest("Incorrect Data");
            if (token is null)
                return Unauthorized();

            try
            {
                new User().desactivar(id, _context);
                return Ok();

            } catch (Exception e) {
                return BadRequest("Something broke");
            }            
        }

        [HttpPut("user/{id}/clave/set")]
        [Allow("sistema")]
        public IActionResult User_SetClave(Token token, int id) 
        {
            if (id <= 0)
                return BadRequest("Incorrect Data");
            if (token is null)
                return Unauthorized();

            try
            {
                string clave = new User().nueva_clave(id, _context);
                return Ok( new {clave = clave} );

            } catch (Exception e) {
                return BadRequest("Something broke");
            }            
        }

        [HttpPut("user/clave")]
        public IActionResult User_CambiarClave([FromForm] Clave req ) 
        {
            if (req is null)
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