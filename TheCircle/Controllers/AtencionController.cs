using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using Microsoft.EntityFrameworkCore;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    public class AtencionController : Controller
    {
        private readonly MyDbContext _context;
        public AtencionController (MyDbContext context)
        {
            _context = context;
        }

        //Crea una atencion medica
        [HttpPost ("api/atencion")]
        public IEnumerable<Atencion> PostAtencion([FromBody] AtencionNueva atencion)
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

                //try {
                    var data = _context.Atenciones.FromSql(query); //manejar errores para que no se caiga
                    return data;
                //} catch (Exception e) {
                    //return new Stack<Atencion>();
                //}          
          }
            return new Stack<Atencion>();
        }

        //Crea una remision medica
        [HttpPost ("api/remision", Name = "PostRemision")]
        public IEnumerable<Remision> PostRemision([FromBody] RemisionNueva remision)
        {
            if (remision != null)
            {
                string query = "DECLARE @id int" + 
                  " EXEC dbo.insert_Remision @atencionM=" + remision.atencionM +
                  ", @institucion=" + remision.institucion +
                  ", @monto=" + remision.monto +
                  ", @id = @id OUTPUT";

                //try
                {
                    var data = _context.Remisiones.FromSql(query); //manejar errores para que no se caiga
                    return data;
                }
                //catch (Exception e)
                {
                    //return new Stack<Remision>();
                }
            }
            return new Stack<Remision>();
        }


        [HttpGet("api/institucion")]
        [ResponseCache(Duration = 60*60)] //1*60 minutos
        public IEnumerable<Institucion> GetInstituciones()
        {
            {
                try {
                    var data = _context.Instituciones.FromSql("EXEC dbo.select_Institucion").ToList();
                    return data;
                } catch (Exception e) {
                    return new Stack<Institucion>();
                }          
            }
        }
    }
}
