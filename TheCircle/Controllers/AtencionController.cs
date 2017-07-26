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
    //[Route("api/atencion")]
    public class AtencionController : Controller
    {
        private readonly MyDbContext _context;
        public AtencionController (MyDbContext context)
        {
            _context = context;
        }

        // POST: api/Enfermedad
        [HttpPost ("api/atencion")]
        public JsonResult PostAtencion([FromBody] AtencionNueva atencion)
        {
          if (atencion != null) {
                if (atencion.diag1 == null)
                {
                    atencion.diag1 = "null";
                }

                if (atencion.diag2 == null)
                {
                    atencion.diag2 = "null";
                }

                string query = "DECLARE @id int "+
                    "EXEC dbo.insert_AtencionM @apadrinado=" +atencion.apadrinado+
                  ", @doctor="+ atencion.doctor +
                  ", @tipo=" + atencion.tipo +
                  ", @diagp=" + atencion.diagp +
                  ", @diag1=" + atencion.diag1 +
                  ", @diag2=" + atencion.diag2 +
                  ", @id = @id OUTPUT";
                
                
                var data = _context.Database.ExecuteSqlCommand(query); //manejar errores para que no se caiga
                
                
            return Json( data);;
          }
          return Json(new {
                state = 0,
                msg = string.Empty
            });
        }

        [HttpGet("api/institucion")]
        public IEnumerable<Institucion> GetInstituciones()
        {
            {
                var data = _context.Instituciones.FromSql("EXEC dbo.select_Institucion").ToList();
                return data;
            }
        }
    }
}
