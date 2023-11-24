using Microsoft.AspNetCore.Mvc;

namespace Actual.Controllers
{
    public class PreguntasFrecuentes : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
