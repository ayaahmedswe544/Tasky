using Microsoft.AspNetCore.Mvc;
using Tasky.Services.IServs;

namespace Tasky.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICatServs _catServs;
        public CategoriesController(ICatServs catServs)
        {
            _catServs = catServs;
        }
        public async Task <IActionResult> Index()
        {
            var Cats = await _catServs.GetAllCategoriesAsync();

            return View(Cats);
        }
    }
}
