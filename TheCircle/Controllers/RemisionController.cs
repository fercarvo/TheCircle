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
    [Route("api/Remision")]
    public class RemisionController : Controller
    {

        private readonly MyDbContext _context;
        public RemisionController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Remision
        [HttpGet]
        public IEnumerable<Remision> Get()
        {
            //using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                var data = _context.Remisiones.FromSql("EXEC dbo.select_Remision").ToList();
                return data;
            }
        }

        // GET: api/Remision/5
        [HttpGet("{id}")]
        public int Get(int id)
        {
            return id;
        }
        
        // POST: api/Remision
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Remision/5
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
