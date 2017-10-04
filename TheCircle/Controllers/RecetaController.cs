using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;
using System;
using TheCircle.Util;
using System.Collections.Generic;
using System.Linq;

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

            var recetas = Receta.ReportByDoctor(fecha, token.data.cedula, _context);
            return Ok(recetas);
        }

        [HttpGet("receta/localidad/fecha")]
        //[ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Client)]
        [APIauth("coordinadorCC")]
        public IActionResult ReportCoordinadorCC(Token token, [FromQuery] Date fecha)
        {
            Receta[] recetas = Receta.ReportLocalidad (token.data.localidad, fecha.desde, fecha.hasta);
            var data = new List<object>();

            foreach (Receta receta in recetas) {
                var items = ItemReceta.ReportReceta(receta.id, _context);
                if (items.Count() > 0)
                    data.Add(new { receta, items });
            }

            return Ok(data);
        }


        //recetas medicas no eliminadas y no despachadas de un doctor.
        [HttpGet("receta/medico/activas")]
        [ResponseCache(Duration = 40, Location = ResponseCacheLocation.Client)]
        [APIauth("medico")]
        public IActionResult GetRecetasByDoctorByStatus(Token token)
        {
            var recetas = Receta.ReportByDoctorByStatus(token.data.cedula, _context);
            return Ok(recetas);
        }


        //recetas a despachar por localidad
        [HttpGet("receta/localidad/pordespachar")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
        [APIauth("asistenteSalud")]
        public IActionResult Get_Recetas_Localidad_Status(Token token)
        {
            var recetas = Receta.ReportLocalidadSinDespachar( token.data.localidad, _context);
            var data = new List<object>();

            foreach (Receta receta in recetas) 
                data.Add( new { receta, items = ItemReceta.ReportReceta(receta.id, _context) });
            
            return Ok(data);
        }

        //recetas con inconsistencia
        [HttpGet("receta/inconsistente")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
        [APIauth("contralor")]
        public IActionResult GetInconsistentes()
        {
            var recetas = Receta.ReportInconsistente();
            var data = new List<object>();

            foreach (Receta receta in recetas)
                data.Add(new { receta, items = ItemDespacho.GetByReceta(receta.id, _context) });

            return Ok(data);
        }


        //recetas despachadas por asistente de salud
        [HttpGet("receta/asistente")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
        [APIauth("asistenteSalud")]
        public IActionResult getRecetasDespachadas(Token token)
        {
            Receta[] recetas = Receta.ReportAsistente(token.data.cedula, _context);
            List<object> data = new List<object>();

            foreach (Receta receta in recetas) 
                data.Add( new {
                    receta = receta,
                    items = ItemDespacho.GetByReceta(receta.id, _context)
                });
            
            return Ok(data); 
        }


        //Crea una receta de farmacia
        /*[HttpPost("receta/apadrinado/{apadrinado}")]
        [APIauth("medico")]
        public IActionResult PostReceta(Token token, int apadrinado)
        {
            if (apadrinado <= 10000)
                return BadRequest("Incorrect Data");
           
            Receta receta = new Receta(apadrinado, token.data.cedula, _context);
            return Ok(receta);
        }*/


        //Crea una receta de farmacia con sus items
        [HttpPost("receta/apadrinado/{apadrinado}")]
        [APIauth("medico")]
        public IActionResult PostItemsReceta(Token token, int apadrinado, [FromBody]ItemReceta.Data[] items)
        {
            if (items is null || items.Length==0 || apadrinado <= 1000)
                return BadRequest();

            var tran = _context.Database.BeginTransaction(); //Se inicia transacción en la BDD

            try 
            {
                Receta receta = new Receta(apadrinado, token.data.cedula, _context);

                foreach (ItemReceta.Data item in items)
                    new ItemReceta(receta.id, item, _context);

                tran.Commit(); //Si todos los items se ingresan correctamente se hace commit
                return Ok(receta);

            } catch (Exception e) {
                tran.Rollback();
                throw new Exception("Error al crear receta con sus items", e);
            }            
        }

        //Se actualiza una receta a despachada, asistente de salud
        [HttpPut("receta/{id}")]
        [APIauth("asistenteSalud")]
        public IActionResult PostDespachoReceta(Token token, int id, [FromBody]ItemDespacho.Data[] items)
        {
            if (items is null)
                return BadRequest();

            var tran = _context.Database.BeginTransaction();
            try {                
                foreach (ItemDespacho.Data item in items) //se insertan en la base de datos todos los items
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