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
    [Route("api/Remision")]
    public class RemisionController : Controller
    {
        // GET: api/Remision
        [HttpGet]
        public IEnumerable <Remision> Get()
        {
            return new Remision[] { new Remision(123,123123,123123,"dasda", 1213.1, 342, 2342), new Remision(123, 123123, 123123, "dasda", 1213.1, 342, 2343) };
        }

        // GET: api/Remision/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
