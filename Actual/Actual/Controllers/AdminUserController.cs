using Microsoft.AspNetCore.Mvc;

namespace Actual.Controllers
{
    public class AdminUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
