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
            string query = "EXEC dbo.insert_Atencion @apadrinado=" + atencion.apadrinado +
              " @doctor=" + atencion.doctor +
              " @tipo=" + atencion.tipo +
              " @diagnosticop=" + atencion.diagnosticop +
              " @diagnostico1=" + atencion.diagnostico1 +
              " @diagnostico2=" + atencion.diagnostico2;

            var data = _context.Database.ExecuteSqlCommand("dbo.insert_Atencion @apadrinado, @doctor, @tipo, @diagnosticop, @diagnostico1, @diagnostico2",
                  parameters: new[] {
                    atencion.apadrinado,
                    atencion.doctor,
                    atencion.tipo,
                    atencion.diagnosticop,
                    atencion.diagnostico1,
                    atencion.diagnostico2
                  }
                );
                
            return Json( new {data: data });;
          }
          return Json(new {
                state = 0,
                msg = string.Empty
            });
        }


    }
}
