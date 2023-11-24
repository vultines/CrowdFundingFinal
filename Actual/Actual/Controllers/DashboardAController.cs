using Microsoft.AspNetCore.Mvc;

namespace Actual.Controllers
{
    public class DashboardAController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
