using Microsoft.AspNetCore.Mvc;
using TheCircle.Models;

namespace TheCircle.Controllers
{

    public class ItemFarmaciaController : Controller
    {
        private readonly MyDbContext _context;
        public ItemFarmaciaController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/ItemFarmacia
        [HttpGet("api/itemfarmacia/{localidad}")]
        public IActionResult GetItems(string localidad)
        {
            ItemFarmacia item = new ItemFarmacia();

            try {
                ItemFarmacia[] stock = item.getAllByLocalidad(localidad, _context);
                return Ok(stock);
            } catch (Exception e) {
                Console.WriteLine(e);
                return BadRequest();
            }
        }
    }
    
}
