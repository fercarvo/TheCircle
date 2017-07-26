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
                /*string query = "EXEC dbo.insert_Atencion @apadrinado=" + atencion.apadrinado +
                  " @doctor=" + atencion.doctor +
                  " @tipo=" + atencion.tipo +
                  " @diagnosticop=" + atencion.diagp +
                  " @diagnostico1=" + atencion.diag1 +
                  " @diagnostico2=" + atencion.diag2;
                  */
                /*
                var data = _context.Database.ExecuteSqlCommand("EXEC dbo.insert_AtencionM @apadrinado, @doctor, @tipo, @diagp, @diag1, @diag2",
                  parameters: new object[] {
                    atencion.apadrinado,
                    atencion.doctor,
                    atencion.tipo,
                    atencion.diagp,
                    atencion.diag1,
                    atencion.diag2
                  }
                );
                */
                
            return Json( atencion);;
          }
          return Json(new {
                state = 0,
                msg = string.Empty
            });
        }


    }
}
