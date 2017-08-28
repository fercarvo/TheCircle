using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using System;
using TheCircle.Util;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    public class AtencionController : Controller
    {
        private readonly MyDbContext _context;
        private Token _validate;

        public AtencionController (MyDbContext context)
        {
            _context = context;
            _validate = new Token();
        }

        //Crea una atencion medica
        [HttpPost ("api/atencion")]
        public IActionResult PostAtencion([FromBody] AtencionRequest request)
        {
            AtencionResponse response = new AtencionResponse();
            Diagnostico temp = new Diagnostico();
            Atencion atencion = new Atencion(); //atencion creada

            if (request == null)
                return BadRequest("Incorrect Data");


            try {
                Token token = _validate.check(Request, new string[] { "medico" });
                atencion = atencion.crear(request, token.data.cedula, token.data.localidad, _context);

                Diagnostico[] diagnosticos = temp.getAllByAtencion(atencion.id, _context);

                response.atencion = atencion;
                response.diagnosticos = diagnosticos;

                return Ok(response);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
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
            Receta receta = new Receta();

            if (request == null)
                return BadRequest("Incorrect Data");
            
            try {
                Token token = _validate.check(Request, new string[] { "medico" });

                receta = receta.crear(request.apadrinado, token.data.cedula, _context);
                return Ok(receta);
            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }             
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

            if (receta == null)
                return BadRequest("Invalid Data");

            try {

                foreach (ItemRecetaRequest item in receta.items) //se insertan en la base de datos todos los items
                    itemReceta.insert(receta.idReceta, item, _context);

                var data = itemReceta.getAllByReceta(receta.idReceta, _context);
                return Ok(data);

            } catch (Exception e) {
                return BadRequest("Somethig broke");
            }            
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
