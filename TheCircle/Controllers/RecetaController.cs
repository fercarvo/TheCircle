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
        public RecetaController (MyDbContext context)
        {
            _context = context;
        }

        //recetas medicas emitidas por un doctor
        [HttpGet("receta/medico/fecha")]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Client)]
        [APIauth("medico")]
        public IActionResult Get_ReporteReceta_Doctor_Date(Token token, [FromQuery] Fecha fecha)
        {
            if (!ModelState.IsValid)
                return BadRequest("Incorrect data");

            try
            {
                List<RecetaTotal> recetas = new RecetaTotal().reporteByDoctor(fecha, token.data.cedula, _context);
                return Ok(recetas);

            } catch (Exception e) {
                return StatusCode(500);
            }
        }

        //recetas medicas no eliminadas y no despachadas de un doctor.
        [HttpGet("receta/medico/activas")]
        [ResponseCache(Duration = 40, Location = ResponseCacheLocation.Client)]
        [APIauth("medico")]
        public IActionResult GetRecetasByDoctorByStatus(Token token)
        {
            try
            {
                List<RecetaTotal> recetas = new RecetaTotal().reporteByDoctorByStatus(token.data.cedula, _context);
                return Ok(recetas);

            } catch (Exception e) {
                return StatusCode(500);
            }
        }

        //recetas a despachar por localidad
        [HttpGet("receta/localidad/pordespachar")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
        [APIauth("asistenteSalud")]
        public IActionResult Get_Recetas_Localidad_Status(Token token)
        {
            try
            {
                var recetas = new RecetaTotal().getAll_Localidad_SinDespachar(token.data.localidad, _context);
                return Ok(recetas);

            } catch (Exception e) {
                return StatusCode(500);
            }
        }

        //recetas despachadas por asistente de salud
        [HttpGet("receta/asistente")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
        [APIauth("asistenteSalud")]
        public IActionResult getRecetasDespachadas(Token token)
        {
            try
            {
                List<RecetaDespacho> recetas = new RecetaDespacho().getBy_Asistente(token.data.cedula, _context);
                return Ok(recetas);

            } catch (Exception e) {
                return StatusCode(500);
            }
        }

        //Crea una receta de farmacia
        [HttpPost("receta/apadrinado/{apadrinado}")]
        [APIauth("medico")]
        public IActionResult PostReceta(Token token, int apadrinado)
        {
            if (apadrinado <= 0)
                return BadRequest("Incorrect Data");
            
            try
            {                
                Receta receta = new Receta().crear(apadrinado, token.data.cedula, _context);
                return Ok(receta);

            } catch (Exception e) {
                return StatusCode(500);
            }
        }


        //Crea una receta de farmacia con sus items
        [HttpPost("receta/{id}")]
        [APIauth("medico")]
        public IActionResult PostItemsReceta(int id, [FromBody]ItemRecetaRequest[] items)
        {
            if (items is null || id <= 0)
                return BadRequest("Invalid Data");

            try 
            {
                new ItemReceta().insert(id, items, _context);
                return Ok();

            } catch (Exception e) {
                return StatusCode(500);
            }
        }


        //Se actualiza una receta a despachada, asistente de salud
        [HttpPut("receta/{recetaId}")]
        [APIauth("asistenteSalud")]
        public IActionResult PostDespachoReceta(Token token, int recetaId, [FromBody]ItemsDespachoRequest[] items)
        {
            if (recetaId <= 0 || items is null)
                return BadRequest("Incorrect Data");

            try
            {
                new ItemDespacho().insert(recetaId, items, token.data.cedula, _context);
                return Ok();

            } catch (Exception e) {
                return StatusCode(500);
            }
        }

        //Elimina un receta de farmacia
        [HttpDelete("receta/{id}")]
        [APIauth("medico")]
        public IActionResult DeleteReceta(int id)
        {
            try
            {
                new Receta().delete(id, _context);
                return Ok();

            } catch (Exception e) {
                return StatusCode(500);
            }
        }
    }
}