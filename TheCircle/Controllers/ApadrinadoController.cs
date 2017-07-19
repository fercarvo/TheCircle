using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using Microsoft.EntityFrameworkCore;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api/Apadrinado")]
    public class ApadrinadoController : Controller
    {

        private readonly MyDbContext _context;
        public ApadrinadoController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Apadrinado
        [HttpGet]
        public IEnumerable<Apadrinado> Get()
        {
            //using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                var data = _context.Apadrinados.FromSql("EXEC dbo.select_Apadrinado").ToList();
                return data;
            }
        }

        // GET: api/Apadrinado/5
        [HttpGet("{cod}", Name = "Get")]
        public IEnumerable<Apadrinado> Get(int cod)
        {
            //using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                string query = "EXEC dbo.select_Apadrinado2 @cod=" + cod;
                var data = _context.Apadrinados.FromSql(query).ToList();
                return data;
            }
        }
        
        // POST: api/Apadrinado
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Apadrinado/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
