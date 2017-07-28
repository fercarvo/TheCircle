using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using Microsoft.EntityFrameworkCore;
using System;

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
        public IEnumerable<Apadrinado> GerApadrinados()
        {
            var data = _context.Apadrinados.FromSql("EXEC dbo.select_Apadrinado3").ToList();
            return data;
        }


        [HttpGet("{cod}")]
        [ResponseCache(Duration = 60)] //cache de 60 segundos
        public IEnumerable<Apadrinado> GetApadrinado(int cod)
        {
            try {
                string query = "EXEC dbo.select_ApadrinadoByCod @cod=" + cod;
                var data = _context.Apadrinados.FromSql(query).ToList();
                return data;
            } catch (Exception e) {
                return new Stack<Apadrinado>();
            }
        }


        [HttpGet("foto/{cod}")]
        [ResponseCache(Duration = 60 * 5)]
        public IActionResult GetApadrinadoFoto(int cod)
        {
            string query = "EXEC dbo.ApadrinadoFotoByCod @cod=" + cod;

            try {
                var data = _context.Fotos.FromSql(query).ToList();
                if (data.Count == 0) {
                    var image2 = System.IO.File.OpenRead("..\\TheCircle\\wwwroot\\images\\ci.png");
                    return File(image2, "image/jpeg");
                } else {
                    var image = System.IO.File.OpenRead("\\\\Guysrv08\\aptifyphoto\\DPHOTO\\Images\\" + data[0].path + "\\" + data[0].name);
                    return File(image, "image/jpeg");
                }
            } catch (Exception e) {
                var image2 = System.IO.File.OpenRead("..\\TheCircle\\wwwroot\\images\\ci.png");
                return File(image2, "image/jpeg");
            }
        }
    }
}
