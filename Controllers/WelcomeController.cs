using Microsoft.AspNetCore.Mvc;

namespace Tasky.Controllers
{
    public class WelcomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
