using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using Microsoft.EntityFrameworkCore;


namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api/cargo")]
    public class CargoController : Controller
    {

        private readonly MyDbContext _context;
        public CargoController (MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Cargo
        [HttpGet]
        public IEnumerable<Cargo> Get()
        {
            //using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                var data = _context.Cargos.FromSql("EXEC dbo.select_Cargo").ToList();
                    return data;
            }
        }
    }
}
