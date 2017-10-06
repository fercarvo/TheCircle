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
        [APIauth("sistema")]
        public IActionResult GetAll()
        {
            UserSafe[] usuarios = UserSafe.GetAll();
            return Ok(usuarios);           
        }

        [HttpGet("user/{cedula}/photo")]
        [ResponseCache(Duration = 60*2, Location = ResponseCacheLocation.Client)]
        [APIauth("medico", "asistenteSalud", "sistema", "bodeguero", "coordinador", "contralor", "coordinadorCC")]
        public IActionResult GetPhoto(Token token, int cedula)
        {
            if (cedula != token.data.cedula)
                return BadRequest();

            string cedula_string = $"{cedula}";
            if (cedula_string.Length == 9)
                cedula_string = $"0{cedula_string}";

            try
            {
                var image = System.IO.File.OpenRead($"\\\\Guysrv11\\Programs\\G_Fotos\\TheCircle\\{cedula_string}.jpg");
                return File(image, "image/jpeg");

            } catch (Exception e) {
                return LocalRedirect("/images/foto.png");
            }
        }

        [HttpGet("user/activos")]
        [APIauth("sistema")]
        public IActionResult GetActivos()
        {
            UserSafe[] usuarios = UserSafe.GetActivos();
            return Ok(usuarios);          
        }

        [HttpGet("user/inactivos")]
        [APIauth("sistema")]
        public IActionResult GetInactivos()
        {
            UserSafe[] usuarios = UserSafe.GetInactivos();
            return Ok(usuarios);          
        }

        [HttpPost("user")]
        public IActionResult New([FromForm]string cedula, [FromForm]string clave)
        {
            if (string.IsNullOrEmpty(cedula) || string.IsNullOrEmpty(clave))
                return BadRequest("Datos incorrectos");

            new Usuario(cedula, clave, _context);
            
            return Ok();
        }


        [HttpPut("user/{cedula}/activar")]
        [APIauth("sistema")]
        public IActionResult Activar(int cedula) 
        {
            if (cedula <= 10000)
                return BadRequest("Incorrect Data");

            Usuario.Activar(cedula, _context);

            UserSafe[] usuarios = UserSafe.GetInactivos();
            return Ok(usuarios);
        }

        [HttpPut("user/{cedula}/desactivar")]
        [APIauth("sistema")]
        public IActionResult Desactivar(int cedula) 
        {
            if (cedula <= 10000)
                return BadRequest("Incorrect Data");

            Usuario.Desactivar(cedula, _context);

            UserSafe[] usuarios = UserSafe.GetActivos();
            return Ok(usuarios);
        }

        [HttpPut("user/{cedula}/clave/set")]
        [APIauth("sistema")]
        public IActionResult User_SetClave(int cedula) 
        {
            if (cedula <= 10000000)
                return BadRequest("Incorrect Data");

            string clave = Usuario.NuevaClave(cedula, _context);
            return Ok( new {clave} );       
        }

        //Ruta para recuperar la clave de un usuario
        [HttpPut("user/recuperarclave")]
        public IActionResult RecuperarClave([FromForm]Data req)
        {
            string mensaje = Usuario.RecuperarClave(req.cedula);
            return Ok(new { mensaje });
        }


        [HttpPut("user/clave")]
        public IActionResult User_CambiarClave([FromForm] Clave req ) 
        {
            if (req is null)
                return BadRequest("Datos incorrectos");

            Usuario.CambiarClave(req.cedula, req.actual, req.nueva, _context);

            return Ok();         
        }

        public class Data {
            public int cedula { get; set; }
            //public string email { get; set; }
        }
    }
}