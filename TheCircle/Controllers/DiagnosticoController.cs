using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api/Diagnostico")]
    public class DiagnosticoController : Controller
    {
        // GET: api/Diagnostico
        [HttpGet]
        public IEnumerable<Diagnostico> Get()
        {
            return new Diagnostico[] { new Diagnostico("abc", 12312), new Diagnostico("bcgd", 324234234)};
        }

        // GET: api/Diagnostico/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
