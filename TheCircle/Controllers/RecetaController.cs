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

            List<RecetaTotal> recetas = RecetaTotal.ReportByDoctor(fecha, token.data.cedula, _context);
            return Ok(recetas);
        }


        //recetas medicas no eliminadas y no despachadas de un doctor.
        [HttpGet("receta/medico/activas")]
        [ResponseCache(Duration = 40, Location = ResponseCacheLocation.Client)]
        [APIauth("medico")]
        public IActionResult GetRecetasByDoctorByStatus(Token token)
        {
            List<RecetaTotal> recetas = new RecetaTotal().reporteByDoctorByStatus(token.data.cedula, _context);
            return Ok(recetas);
        }


        //recetas a despachar por localidad
        [HttpGet("receta/localidad/pordespachar")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
        [APIauth("asistenteSalud")]
        public IActionResult Get_Recetas_Localidad_Status(Token token)
        {
            var recetas = new RecetaTotal().getAll_Localidad_SinDespachar(token.data.localidad, _context);
            return Ok(recetas);
        }


        //recetas despachadas por asistente de salud
        [HttpGet("receta/asistente")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
        [APIauth("asistenteSalud")]
        public IActionResult getRecetasDespachadas(Token token)
        {
            List<RecetaDespacho> recetas = new RecetaDespacho().getBy_Asistente(token.data.cedula, _context);
            return Ok(recetas);
        }


        //Crea una receta de farmacia
        [HttpPost("receta/apadrinado/{apadrinado}")]
        [APIauth("medico")]
        public IActionResult PostReceta(Token token, int apadrinado)
        {
            if (apadrinado <= 10000)
                return BadRequest("Incorrect Data");
           
            Receta receta = Receta.New(apadrinado, token.data.cedula, _context);
            //Receta receta = new Receta(apadrinado, token,data.cedula, _context);
            return Ok(receta);
        }


        //Crea una receta de farmacia con sus items
        [HttpPost("receta/{id}")]
        [APIauth("medico")]
        public IActionResult PostItemsReceta(int id, [FromBody]ItemRecetaRequest[] items)
        {
            if (items is null || id <= 0)
                return BadRequest("Invalid Data");

            ItemReceta.Insert(id, items, _context);
            return Ok();      
        }


        //Crea una receta de farmacia con sus items
        [HttpPost("receta2/{id}")]
        [APIauth("medico")]
        public IActionResult PostItemsReceta2(int id, [FromBody]ItemReceta.Data[] items)
        {
            if (items is null)
                return BadRequest();

            var tran = _context.Database.BeginTransaction(); //Se inicia transacción en la BDD
            try 
            {
                foreach (ItemReceta.Data item in items)
                    new ItemReceta(id, item, _context);

                tran.Commit(); //Si todos los items se ingresan correctamente se hace commit
                return Ok();

            } catch (Exception e) {
                tran.Rollback();
                return BadRequest("Error al ingresar los itemsReceta");
            }            
        }


        //Se actualiza una receta a despachada, asistente de salud
        [HttpPut("receta/{recetaId}")]
        [APIauth("asistenteSalud")]
        public IActionResult PostDespachoReceta(Token token, int recetaId, [FromBody]ItemsDespachoRequest[] items)
        {
            //if (items is null)
            if (recetaId <= 0 || items is null)
                return BadRequest("Incorrect Data");

            ItemDespacho.Insert(recetaId, items, token.data.cedula, _context);
            return Ok();
        }


        //Se actualiza una receta a despachada, asistente de salud
        [HttpPut("receta2/{id}")]
        [APIauth("asistenteSalud")]
        public IActionResult PostDespachoReceta2(Token token, int id, [FromBody]ItemsDespacho.Data[] items)
        {
            if (items is null)
                return BadRequest();

            var tran = _context.Database.BeginTransaction();
            try {                
                foreach (ItemsDespacho.Data item in items) //se insertan en la base de datos todos los items
                    new ItemDespacho(item, token.data.cedula, _context);

                Receta.UpdateDespachada(id, _context);
                tran.Commit();
                return Ok();

            } catch (Exception e) {
                tran.Rollback();
                return BadRequest("Error al despachar la receta");
            }
        }


        //Elimina un receta de farmacia
        [HttpDelete("receta/{id}")]
        [APIauth("medico")]
        public IActionResult DeleteReceta(int id)
        {
            Receta.Delete(id, _context);
            return Ok();
        }
    }
}