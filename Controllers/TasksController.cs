using Microsoft.AspNetCore.Mvc;

namespace Tasky.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult Index()
        {
            return Content("adjfa");
        }
    }
}
