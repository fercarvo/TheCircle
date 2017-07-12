using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api/Diagnostico")]
    public class DiagnosticoController : Controller
    {
        // GET: api/Diagnostico
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Diagnostico/5
        [HttpGet("{id}", Name = "Get")]
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
