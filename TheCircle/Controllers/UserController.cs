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
        //[APIauth("sistema")]
        public IActionResult Get_All_Users()
        {
            UserSafe[] usuarios = new UserSafe().getAll(_context);
            return Ok(usuarios);           
        }

        [HttpGet("user/{cedula}/photo")]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)]
        [APIauth("medico", "asistenteSalud", "sistema", "bodeguero", "coordinador", "contralor", "coordinadorCC")]
        public IActionResult Get_Foto(Token token, int cedula)
        {
            if (cedula != token.data.cedula)
                return BadRequest();

            try
            {
                var image = System.IO.File.OpenRead($"\\\\Guysrv11\\Programs\\G_Fotos\\TheCircle\\{token.data.cedula}.jpg");
                return File(image, "image/jpeg");

            } catch (Exception e) {
                return LocalRedirect("/images/ci.png");
            }
        }

        [HttpGet("user/activos")]
        [APIauth("sistema")]
        [ResponseCache(Duration = 1, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_All_Users_Activos()
        {
            UserSafe[] usuarios = new UserSafe().getActivos(_context);
            return Ok(usuarios);          
        }

        [HttpGet("user/inactivos")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        [APIauth("sistema")]
        public IActionResult Get_All_Users_Inactivos()
        {
            UserSafe[] usuarios = new UserSafe().getInactivos(_context);
            return Ok(usuarios);          
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


        [HttpPut("user/{cedula}/activar")]
        [APIauth("sistema")]
        public IActionResult User_Activar(int cedula) 
        {
            if (cedula <= 10000)
                return BadRequest("Incorrect Data");

            new User().activar(cedula, _context);
            return Ok();          
        }

        [HttpPut("user/{cedula}/desactivar")]
        [APIauth("sistema")]
        public IActionResult User_Desactivar(int cedula) 
        {
            if (cedula <= 10000)
                return BadRequest("Incorrect Data");

            try
            {
                new User().desactivar(cedula, _context);
                return Ok();

            } catch (Exception e) {
                return BadRequest("Something broke");
            }            
        }

        [HttpPut("user/{cedula}/clave/set")]
        [APIauth("sistema")]
        public IActionResult User_SetClave(int cedula) 
        {
            if (cedula <= 0)
                return BadRequest("Incorrect Data");

            try
            {
                string clave = new User().nueva_clave(cedula, _context);
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