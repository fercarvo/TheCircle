using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using System;
using TheCircle.Util;
using System.Collections.Generic;

namespace TheCircle.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class RecetaController : Controller
    {
        private readonly MyDbContext _context;
        private Token _validate;

        public RecetaController (MyDbContext context)
        {
            _context = context;
            _validate = new Token();
        }

        //recetas medicas emitidas por un doctor
        [HttpGet("receta/medico/fecha")]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)] //cache de 60*60 segundos, para evitar sobrecarga de la BDD
        public IActionResult Get_ReporteReceta_Doctor_Date([FromQuery] Fecha fecha)
        {
            if (!ModelState.IsValid)
                return BadRequest("Incorrect data");

            try {

                Token token = _validate.check(Request, new string[] { "medico" });

                List<RecetaTotal> recetas = new RecetaTotal().reporteByDoctor(fecha, token.data.cedula, _context);
                return Ok(recetas);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
            
        }

        //recetas medicas no eliminadas y no despachadas de un doctor.
        [HttpGet("receta/medico/activas")]
        [ResponseCache(Duration = 40, Location = ResponseCacheLocation.Client)] //cache de 40 segundos, para evitar sobrecarga de la BDD
        public IActionResult GetRecetasByDoctorByStatus()
        {
            try
            {
                Token token = _validate.check(Request, new string[] { "medico" });

                List<RecetaTotal> recetas = new RecetaTotal().reporteByDoctorByStatus(token.data.cedula, _context);
                return Ok(recetas);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
        }

        //recetas a despachar por localidad
        [HttpGet("receta/localidad/pordespachar")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        public IActionResult Get_Recetas_Localidad_Status()
        {
            try
            {
                Token token = _validate.check(Request, new string[] { "asistenteSalud" });

                var recetas = new RecetaTotal().getAll_Localidad_SinDespachar(token.data.localidad, _context);
                return Ok(recetas);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
        }

        //recetas despachadas por asistente de salud
        [HttpGet("receta/asistente")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)] //cache de 10 segundos
        public IActionResult getRecetasDespachadas()
        {
            try
            {
                Token token = _validate.check(Request, new string[] { "asistenteSalud" });

                List<RecetaDespacho> recetas = new RecetaDespacho().getBy_Asistente(token.data.cedula, _context);
                return Ok(recetas);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }
        }

        //Crea una receta de farmacia
        [HttpPost("receta/apadrinado/{apadrinado}")]
        public IActionResult PostReceta(int apadrinado)
        {
            if (apadrinado <= 0)
                return BadRequest("Incorrect Data");
            
            try
            {                
                Token token = _validate.check(Request, new string[] { "medico" });

                Receta receta = new Receta().crear(apadrinado, token.data.cedula, _context);
                return Ok(receta);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }             
        }


        //Crea una receta de farmacia con sus items
        [HttpPost("receta/{id}")]
        public IActionResult PostItemsReceta(int id, [FromBody]ItemRecetaRequest[] items)
        {
            ItemReceta itemReceta = new ItemReceta();

            if (items == null || id <= 0)
                return BadRequest("Invalid Data");

            try
            {
                _validate.check(Request, new string[] { "medico" });

                foreach (ItemRecetaRequest item in items) //se insertan en la base de datos todos los items
                    itemReceta.insert(id, item, _context);

                var data = itemReceta.getAllByReceta(id, _context);
                return Ok(data);

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }            
        }


        //Se actualiza una receta a despachada, asistente de salud
        [HttpPut("receta/{id}")]
        public IActionResult PostDespachoReceta(int id, [FromBody]ItemsDespachoRequest[] items) {
            ItemsDespachoRequest i = new ItemsDespachoRequest();

            if (id == 0 || items == null)
                return BadRequest("Incorrect Data");

            try
            {
                Token token = _validate.check(Request, new string[] { "asistenteSalud" });

                foreach(ItemsDespachoRequest item in items) { //Se insertan todos los despachos
                    i.insert(item, token.data.cedula, _context);
                }

                new Receta().update_despachada(id, _context);
                return Ok();

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }    
            
        }

        //Elimina un receta de farmacia
        [HttpDelete("receta/{id}")]
        public IActionResult DeleteReceta(int id)
        {
            try
            {
                _validate.check(Request, new string[] {"medico"});

                new Receta().delete(id, _context);
                return Ok();

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }    
        } 

    }
}