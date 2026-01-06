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
        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm = null, string? sortOrder = "asc", int pageNumber = 1, int pageSize = 10)

        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var Cats = await _catServs.GetAllCategoriesAsync(userId, searchTerm, sortOrder, pageNumber, pageSize);
            int count = await _catServs.GetCategoriesCountAsync(userId, searchTerm);
            var Catlist = new CatListVM
            {
                CatsVM = Cats.ToList(),
                TotalCats = count,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortOrder = sortOrder,
                SearchTerm = searchTerm
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
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            var catVM = await _catServs.GetCategoryByIdAsync(id);
            if (catVM != null)
            {
                if (catVM.AppUserId != userId)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    EditCatVM editCatVM = new EditCatVM
                    {
                        Id = catVM.Id,
                        Name = catVM.Name,
                        Description = catVM.Description
                    };
                    return View(editCatVM);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditCatVM editCatVM)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            editCatVM.AppUserId = userId;
            ModelState.Remove(nameof(editCatVM.AppUserId));
            if (ModelState.IsValid)
            {
                CatVM catvm = new CatVM
                {
                    Id = editCatVM.Id,
                    Name = editCatVM.Name,
                    Description = editCatVM.Description,
                    AppUserId = editCatVM.AppUserId
                };
                var result = await _catServs.UpdateCategoryAsync(catvm);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Failed to update category.");
            }
            return View(editCatVM);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var catVM = await _catServs.GetCategoryByIdAsync(id);
            if (catVM != null)
            {
                if (catVM.AppUserId != userId)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    await _catServs.DeleteCategoryAsync(id);
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("index");
        }
    }
}
