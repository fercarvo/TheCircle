using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api/Foto")]
    public class FotoCt : Controller
    {

        private readonly MyDbContext _context;
        public FotoCt(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Foto/{{id}}
        [HttpGet("{cod}")]
        [ResponseCache(Duration = 60*5)]
        public IActionResult Get(int cod)
        {
            string query = "EXEC dbo.ApadrinadoFotoByCod @cod=" + cod;
            
            var data = _context.Fotos.FromSql(query).ToList();
            //string ruta = "C:\\\\Guysrv08\\aptifyphoto\\DPHOTO\\Images\\" + data[0].path + "\\" + data[0].name;

            if (data.Count == 0) {
                var image2 = System.IO.File.OpenRead("..\\wwwroot\\images\\ci.png");
                return File(image2, "image/jpeg");
            }
            var image = System.IO.File.OpenRead("\\\\Guysrv08\\aptifyphoto\\DPHOTO\\Images\\" + data[0].path + "\\" + data[0].name);
            return File(image, "image/jpeg");
        }

    }
}


