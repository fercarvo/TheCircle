using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api/Diagnostico")]
    public class CargoController : Controller
    {

        private readonly MyDbContext _context;
        public CargoController (MyDbContext context)
        {
            _context = context;
        }



        // GET: api/Diagnostico
        [HttpGet]
        public IEnumerable<Cargo> Get()
        {
            Console.WriteLine("holaaaaaa");
            Console.WriteLine("holaaaaaa");
            Console.WriteLine("holaaaaaa");
            Console.WriteLine("holaaaaaa");
            Console.WriteLine("holaaaaaa");
            Console.WriteLine("holaaaaaa");
            Console.WriteLine("holaaaaaa");
            using (var context = _context)
            //using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                var data = context.Cargos.FromSql("EXEC dbo.select_Cargo").ToList();
                
                    return data;
                
            }
        }
        /*
        public IEnumerable<Diagnostico> Get()
        {
            return new Diagnostico[] { new Diagnostico("abc", 12312), new Diagnostico("bcgd", 324234234)};
        }
        */

        // GET: api/Diagnostico/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            await _context.Database.ExecuteSqlCommandAsync("insert_Cargo @tipo",
        parameters: new[] { "hello"});
            return "das";
        }

        // POST: api/Diagnostico
        [HttpPost]
        public void Post([FromBody]string value)
        {

        }
        
        // PUT: api/Diagnostico/5
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
