using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using System;

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
        public IActionResult PostAtencion([FromBody] AtencionRequest request)
        {
            AtencionResponse response = new AtencionResponse();
            Diagnostico temp = new Diagnostico();
            Atencion atencion = new Atencion(); //atencion creada

            if (request != null) {
                try {
                    atencion = atencion.crear(request, _context);
                    Diagnostico[] diagnosticos = temp.getAllByAtencion(atencion.id, _context);

                    response.atencion = atencion;
                    response.diagnosticos = diagnosticos;

                    return Ok(response);

                } catch (Exception e) {
                    Console.WriteLine(e);
                    return BadRequest("Somethig broke");
                }

            } else {
                return BadRequest("Incorrect Data");
            }
        }

        //Crea una remision medica
        [HttpPost ("api/remision")]
        public IActionResult PostRemision([FromBody] RemisionRequest request)
        {
            Remision remision = new Remision();
            if (request != null) {

                try {
                    remision = remision.crear(request, _context);
                    return Ok(remision);
                } catch (Exception e) {
                    Console.WriteLine(e);
                    BadRequest("Something Broke");
                }
            }
            return BadRequest("Incorrect Data");
        }

        //Crea una receta de farmacia
        [HttpPost ("api/receta")]
        public IActionResult PostReceta([FromBody] RecetaRequest request)
        {
            if (request != null) {
                Receta receta = new Receta();
                
                try {
                    receta = receta.crear(request, _context);
                    return Ok(receta);
                } catch (Exception e) {
                    Console.WriteLine(e);
                    BadRequest("Something broke");
                }
            }
            return BadRequest("Incorrect Data");
        }

        //Crea una receta de farmacia
        [HttpDelete("api/receta/{id}")]
        public IActionResult DeleteReceta(int id)
        {
            Receta receta = new Receta();

            try {
                receta.delete(id, _context);
                return Ok();
            } catch (Exception e) {
                return BadRequest("Something broke");
            }
        }


        //Crea una receta de farmacia
        [HttpPost ("api/itemsreceta")]
        public IActionResult PostItemsReceta([FromBody] RecetaItemsRequest receta)
        {
            ItemReceta itemReceta = new ItemReceta();

            if (receta != null) {
                foreach (ItemRecetaRequest item in receta.items) { //se insertan en la base de datos todos los items
                    itemReceta.insert(receta.idReceta, item, _context);
                }

                ItemReceta[] data = itemReceta.getAllByReceta(receta.idReceta, _context);
                if (data != null) {
                    return Ok(data);
                }
                return BadRequest("Somethig broke");
            }
            return BadRequest("Invalid Data");
        }

        [HttpGet("api/institucion")]
        [ResponseCache(Duration = 60 * 60 * 48, Location = ResponseCacheLocation.Client)] //cache de 60 * 60 * 48 segundos = 48 horas
        public IActionResult GetInstituciones()
        {
            Institucion institucion = new Institucion();

            try {
                Institucion[] instituciones = institucion.getAll(_context);
                return Ok(instituciones);
            } catch (Exception e) {
                return BadRequest("Somethig broke");
            }
        }
    }
}
