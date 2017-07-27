using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using Microsoft.EntityFrameworkCore;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api/apadrinado")]
    public class ApadrinadoController : Controller
    {

        private readonly MyDbContext _context;
        public ApadrinadoController(MyDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IEnumerable<Apadrinado> Get()
        {
            //using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                var data = _context.Apadrinados.FromSql("EXEC dbo.select_Apadrinado3").ToList();
                return data;
            }
        }


        [HttpGet("{cod}")]
        [ResponseCache(Duration = 60)] //60 segundos
        public IEnumerable<Apadrinado> GetApadrinado(int cod)
        {
            //using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                string query = "EXEC dbo.select_ApadrinadoByCod @cod=" + cod;
                var data = _context.Apadrinados.FromSql(query).ToList();
                return data;
            }
        }


        [HttpGet("foto/{cod}")]
        [ResponseCache(Duration = 60 * 5)]
        public IActionResult GetApadrinadoFoto(int cod)
        {
            string query = "EXEC dbo.ApadrinadoFotoByCod @cod=" + cod;

            var data = _context.Fotos.FromSql(query).ToList();
            //string ruta = "C:\\\\Guysrv08\\aptifyphoto\\DPHOTO\\Images\\" + data[0].path + "\\" + data[0].name;

            if (data.Count == 0)
            {
                var image2 = System.IO.File.OpenRead("..\\wwwroot\\images\\ci.png");
                return File(image2, "image/jpeg");
            }
            var image = System.IO.File.OpenRead("\\\\Guysrv08\\aptifyphoto\\DPHOTO\\Images\\" + data[0].path + "\\" + data[0].name);
            return File(image, "image/jpeg");
        }


    }
}
