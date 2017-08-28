using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TheCircle.Models;
using TheCircle.Util;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    public class DespachoRecetaController : Controller
    {
        private readonly MyDbContext _context;
        private Token _validate;

        public DespachoRecetaController(MyDbContext context)
        {
            _context = context;
            _validate = new Token();
        }

        [HttpGet("api/receta")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        public IActionResult Get_Recetas_Localidad_Status([FromQuery] int status) {
            var rt = new RecetaTotal();

            if (status !=0 && status !=1 )
                return BadRequest();

            try {

                Token token = _validate.check(Request, new string[] { "asistenteSalud" });

                var recetas = rt.getAllByLocalidadByStatus(token.data.localidad, status, _context);
                return Ok(recetas);

            } catch (Exception e) {
                return BadRequest("Something broke");
            }
        }


        [HttpGet("api/despacho/receta")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        public IActionResult getRecetasDespachadas() {

            RecetaDespacho rd = new RecetaDespacho();

            try {

                Token token = _validate.check(Request, new string[] { "asistenteSalud" });

                List<RecetaDespacho> recetas = rd.getBy_Asistente(token.data.cedula, _context);
                return Ok(recetas);
            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        [HttpPost("api/despacho/receta")]
        public IActionResult PostDespachoReceta([FromBody] DespachoRecetaRequest request) {
            ItemsDespachoRequest i = new ItemsDespachoRequest();
            ItemDespacho id = new ItemDespacho();

            if (request == null)
                return BadRequest("Incorrect Data");

            try {

                Token token = _validate.check(Request, new string[] { "asistenteSalud" });

                foreach(ItemsDespachoRequest item in request.items) { //Se insertan todos los despachos
                    i.insert(item, token.data.cedula, _context);
                }

                id.update_RecetaDespachada(request.id, _context); //Se actualiza la receta a despachada
                return Ok();
            } catch (Exception e) {
                return BadRequest("Something broke");
            }
            
        }
    }
}
