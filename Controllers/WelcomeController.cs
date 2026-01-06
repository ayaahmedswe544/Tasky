using Microsoft.AspNetCore.Mvc;

namespace Tasky.Controllers
{
    public class WelcomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Home");
            }
            
            return View();
        }
    }
}
