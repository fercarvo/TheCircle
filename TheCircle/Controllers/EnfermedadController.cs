using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using Microsoft.EntityFrameworkCore;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api/enfermedad")]
    public class EnfermedadController : Controller
    {

        private readonly MyDbContext _context;
        public EnfermedadController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Enfermedad
        [HttpGet]
        [ResponseCache(Duration = 60 * 120)] //1*120 minutos
        public IEnumerable<Enfermedad> Get()
        {
            //using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                var data = _context.Enfermedades.FromSql("EXEC dbo.select_Enfermedad").ToList();
                return data;
            }
        }

      
        // POST: api/Enfermedad
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Enfermedad/5
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
