using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Tasky.Models;
using Tasky.Services.IServs;
using Tasky.VMS.Category;
using Tasky.VMS.TaskVMs;

namespace Tasky.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICatServs _catServs;
        public CategoriesController(ICatServs catServs)
        {
            _catServs = catServs;
        }
        //        public async Task<IActionResult> Index(
        //int? categoryId = null,
        //PriorityLevel? priority = null,
        //bool overdue = false,
        //string? searchTerm = null,
        //string? sortOrder = "asc",
        //int pageNumber = 1,
        //int pageSize = 10)
        [HttpGet]
        public async Task<IActionResult> Index( string? searchTerm = null, string? sortOrder = "asc", int pageNumber = 1, int pageSize = 10)

        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var Cats = await _catServs.GetAllCategoriesAsync(userId, searchTerm, sortOrder, pageNumber, pageSize);
            int count = await _catServs.GetCategoriesCountAsync(userId, searchTerm);
            var Catlist = new CatListVM
            {
                CatsVM = Cats.ToList(),
                TotalCats = count,
                PageNumber=pageNumber,
                PageSize=pageSize,
                SortOrder=sortOrder,
                SearchTerm=searchTerm
            };


            return View(Catlist);
        }

        public async Task<IActionResult> IsCatNameInUse(string name)
        {
            var cat = await _catServs.IsNameInUseAsync(name);
            if (cat)
            {
                return Json($"Category name {name} is already in use.");
            }
            return Json(true);

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateVM createVM)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            createVM.AppUserId = userId;
            ModelState.Remove(nameof(createVM.AppUserId));

            if (ModelState.IsValid)
            {
                var result = await _catServs.CreateCategoryAsync(createVM);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Failed to create category.");
            }
            return View(createVM);
        }

    }
}
