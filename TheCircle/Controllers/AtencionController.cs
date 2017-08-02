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
        public IActionResult PostAtencion([FromBody] AtencionRequest request)
        {
            AtencionResponse response = new AtencionResponse();
            Diagnostico temp = new Diagnostico();
            Atencion atencion = new Atencion(); //atencion creada

            if (request != null) {

                atencion = atencion.crear(request, _context);
                if (atencion != null) {
                    Diagnostico[] diagnosticos = temp.getAllByAtencion(atencion.id, _context);
                    response.atencion = atencion;
                    response.diagnosticos = diagnosticos;
                    return Ok(response);
                } else {
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
                remision = remision.crear(request, _context);
                if (remision != null) {
                    return Ok(remision);
                } else {
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
                    BadRequest(e);
                }
            }
            return BadRequest("Incorrect Data");
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
        [ResponseCache(Duration = 60*60)] //1*60 minutos
        public IActionResult GetInstituciones()
        {
            Institucion institucion = new Institucion();
            Institucion[] instituciones = institucion.getAll(_context);
            if (instituciones != null) {
                return Ok(instituciones);
            }
            return BadRequest("Somethig broke");
        }
    }
}
