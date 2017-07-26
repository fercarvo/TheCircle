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
    [Route("api/atencion")]
    public class AtencionController : Controller
    {
        private readonly MyDbContext _context;
        public AtencionController (MyDbContext context)
        {
            _context = context;
        }

        // POST: api/Enfermedad
        [HttpPost]
        public JsonResult Post([FromBody] AtencionNueva atencion)
        {
          if (atencion != null) {
                string query = "DECLARE @id int "+
                    "EXEC dbo.insert_AtencionM @apadrinado="+atencion.apadrinado+
                  ", @doctor="+ 705565656 +
                  ", @tipo=" + atencion.tipo +
                  ", @diagp=" + atencion.diagp +
                  ", @diag1=" + atencion.diag1 +
                  ", @diag2=" + atencion.diag2 +
                  ", @id = @id OUTPUT";
                
                
                var data = _context.Database.ExecuteSqlCommand(query);
                
                
            return Json( atencion);;
          }
          return Json(new {
                state = 0,
                msg = string.Empty
            });
        }


    }
}
