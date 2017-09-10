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
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Client)]
        [APIauth("sistema")]
        public IActionResult Get_All_Users()
        {
            UserSafe[] usuarios = UserSafe.GetAll(_context);
            return Ok(usuarios);           
        }

        [HttpGet("user/{cedula}/photo")]
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Client)]
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
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Client)]
        public IActionResult Get_All_Users_Activos()
        {
            UserSafe[] usuarios = UserSafe.GetActivos(_context);
            return Ok(usuarios);          
        }

        [HttpGet("user/inactivos")]
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Client)] 
        public IActionResult Get_All_Users_Inactivos()
        {
            UserSafe[] usuarios = UserSafe.GetInactivos(_context);
            return Ok(usuarios);          
        }

        [HttpPost("user")]
        public IActionResult Login_create([FromForm]string cedula, [FromForm]string clave)
        {
            if (string.IsNullOrEmpty(cedula) || string.IsNullOrEmpty(clave))
                return BadRequest("Datos incorrectos");

            User user = new User(cedula, clave, _context);

            if (user is null)
                return BadRequest()

            return Ok();
        }


        [HttpPut("user/{cedula}/activar")]
        [APIauth("sistema")]
        public IActionResult User_Activar(int cedula) 
        {
            if (cedula <= 10000)
                return BadRequest("Incorrect Data");

            User.Activar(cedula, _context);

            UserSafe[] usuarios = UserSafe.GetInactivos(_context);
            return Ok(usuarios);
        }

        [HttpPut("user/{cedula}/desactivar")]
        [APIauth("sistema")]
        public IActionResult User_Desactivar(int cedula) 
        {
            if (cedula <= 10000)
                return BadRequest("Incorrect Data");

            User.Desactivar(cedula, _context);

            UserSafe[] usuarios = UserSafe.GetActivos(_context);
            return Ok(usuarios);
        }

        [HttpPut("user/{cedula}/clave/set")]
        [APIauth("sistema")]
        public IActionResult User_SetClave(int cedula) 
        {
            if (cedula <= 10000000)
                return BadRequest("Incorrect Data");

            try
            {
                string clave = User.Nueva_clave(cedula, _context);
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
                User.NuevaClave(req.cedula, req.actual, req.nueva, _context);
                return Ok();

            } catch (Exception e) {
                return BadRequest("No se pudo cambiar la contraseña");
            }            
        }
    }
}