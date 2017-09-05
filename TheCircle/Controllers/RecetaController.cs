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

                Token token = _validate.check(Request, new[] { "medico" });

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
                Token token = _validate.check(Request, new[] { "medico" });

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
                Token token = _validate.check(Request, new[] { "asistenteSalud" });

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
                Token token = _validate.check(Request, new[] { "asistenteSalud" });

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
                Token token = _validate.check(Request, new[] { "medico" });

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
            if (items == null || id <= 0)
                return BadRequest("Invalid Data");

            try 
            {
                _validate.check(Request, new[] { "medico" });

                new ItemReceta().insert(id, items, _context); //Se insertan los items que vienen del front
                return Ok();

            } catch (Exception e) {
                if (e is TokenException)
                    return Unauthorized();
                return BadRequest("Something broke");
            }            
        }


        //Se actualiza una receta a despachada, asistente de salud
        [HttpPut("receta/{recetaId}")]
        public IActionResult PostDespachoReceta(int recetaId, [FromBody]ItemsDespachoRequest[] items)
        {
            if (recetaId == 0 || items == null)
                return BadRequest("Incorrect Data");

            try
            {
                Token token = _validate.check(Request, new[] { "asistenteSalud" });

                new ItemDespacho().insert(recetaId, items, token.data.cedula, _context);
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
                _validate.check(Request, new[] {"medico"});

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