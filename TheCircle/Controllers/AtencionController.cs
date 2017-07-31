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
        public IActionResult PostAtencion([FromBody] AtencionNueva atencion)
        {


            AtencionDiagnostico a = new AtencionDiagnostico();

            if (atencion != null) {

                string query = "DECLARE @id int "+
                    "EXEC dbo.insert_AtencionM @apadrinado=" +atencion.apadrinado+
                    ", @doctor="+ atencion.doctor +
                    ", @tipo=" + atencion.tipo +
                    ", @id = @id OUTPUT";

                try {
                    var atencionesDB = _context.Atenciones.FromSql(query); //Retorna la AtencionM creada
                    a.atencion = atencionesDB.First(); //Atencion creada

                    string query2 = "EXEC dbo.select_DiagnosticoByAtencion @atencion=" + a.atencion.id;
                    var diagnosticosDB = _context.Diagnosticos.FromSql(query2); //Retorna los diagnosticos de esa AtencionM

                    a.diagnosticos = diagnosticosDB.Select(s => new Diagnostico (s.id, s.enfermedadCod, s.enfermedadNombre)).ToList();

                    if (atencionesDB.Count() == 0)
                    {
                        return NotFound();
                    }
                    return Ok(a);

                } catch (Exception e) {
                    return BadRequest(atencion);
                }
            } else {
                return BadRequest(atencion);
            }
        }

        //Crea una atencion medica
        [HttpPost ("api/atencion2")]
        public IActionResult PostAtencion([FromBody] AtencionNueva atencionN)
        {
            AtencionResponse ar = new AtencionResponse();
            if (atencionN) {
              Atencion atencion = new Atencion(atencionN, _context);
              ar.atencion;

              if (atencion) {
                  Diagnostico[] diagnosticos = Diagnostico.getAllByAtencion(atencion.id, _context);
                  ar.diagnosticos;
                  //return Ok(ar);
                  return Ok(atencion, diagnosticos);
              } else {
                  return BadRequest("Somethig broke");
              }
            } else {
                return BadRequest("Incorrect Data");
            }
        }

        //Crea una remision medica
        [HttpPost ("api/remision")]
        public IActionResult PostRemision([FromBody] RemisionNueva remision)
        {
            if (remision) {
                Remision r = new Remision(remision, _context);
                if (r) {
                    return Ok(r);
                } else {
                    BadRequest("Something Broke");
                }
            }
            return BadRequest("Incorrect Data");
        }

        //Crea una receta de farmacia
        [HttpPost ("api/receta")]
        public IActionResult PostReceta([FromBody] RecetaNueva request)
        {
            if (request)
            {
                try {
                    Receta receta = new Receta(request, _context);
                    return Ok(receta);
                } catch (Exception e) {
                    BadRequest(e);
                }

                /*if (receta) {
                    return Ok(receta);
                } else {
                    BadRequest("Something Broke");
                }*/
            }
            return BadRequest("Incorrect Data");
        }

        //Crea una receta de farmacia
        [HttpPost ("api/itemsreceta")]
        public IActionResult PostItemsReceta([FromBody] RecetaNuevaItems receta)
        {
            if (receta) {
                foreach (ItemRecetaNuevo item in receta.items) {
                    ItemReceta.insert(receta.idReceta, item, _context);
                }

                ItemReceta[] data = ItemReceta.getAllByReceta(receta.idReceta, _context);
                if (data) {
                    return Ok(data);
                }
                return BadRequest("Somethig broke");
            }
            return BadRequest("Invalid Data");
        }

        [HttpGet("api/institucion")]
        [ResponseCache(Duration = 60*60)] //1*60 minutos
        public IActionResult GetInstituciones()
        //public IEnumerable<Institucion> GetInstituciones()
        {
            Institucion[] instituciones = Institucion.getAll(_context);
            if (instituciones) {
                return Ok(instituciones);
            }
            return BadRequest("Somethig broke");
            /*try {
                var data = _context.Instituciones.FromSql("EXEC dbo.select_Institucion").ToList();
                return data;
                //return Ok(data);
            } catch (Exception e) {
                return new Stack<Institucion>();
                //return BadRequest("Somethig broke");
            }*/
        }
    }
}
