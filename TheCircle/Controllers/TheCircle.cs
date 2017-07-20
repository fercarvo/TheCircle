using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TheCircle.Controllers.views
{
    public class TheCircle : Controller
    {
        // GET: /TheCircle/
        // GET: /
        public IActionResult Index()
        {
            return View();
        }
    }
}
