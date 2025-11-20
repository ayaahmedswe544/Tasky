using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Tasky.Models;
using Tasky.Repositories.IRepos;
using Tasky.Services.IServs;
using Tasky.Services.Servs;
using Tasky.VMS.TaskVMs;

namespace Tasky.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskServs _taskServs;
        private readonly ICatServs _categoryServ;
        public TasksController(ITaskServs taskServs, ICatServs catServs)
        {
            _taskServs = taskServs;
            _categoryServ = catServs;

        }

        [HttpGet]
        public async Task<IActionResult> Index(
    int? categoryId = null,
    PriorityLevel? priority = null,
    bool overdue = false,
    string? searchTerm = null,
    string? sortOrder = "asc",
    int pageNumber = 1,
    int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var name = User.FindFirstValue(ClaimTypes.Name);
            var tasks = await _taskServs.GetAllTasksAsync(
                userId,
                categoryId,
                priority,
                overdue,
                searchTerm,
                sortOrder,
                pageNumber,
                pageSize
            );

            var totalCount = await _taskServs.GetTotalTaskCountAsync(
                userId,
                categoryId,
                priority,
                overdue,
                searchTerm
            );

            var vm = new TaskListVM
            {
                Tasks = tasks,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalTasks = totalCount,
                SortOrder = sortOrder,
                SearchTerm = searchTerm,
                CategoryId = categoryId,
                Priority = priority
            };

            ViewBag.Categories = await _categoryServ.GetAllCategoriesAsync();
            ViewBag.SelectedCategory = categoryId;
            ViewBag.SelectedPriority = priority;
            ViewBag.Overdue = overdue;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SortOrder = sortOrder;
            ViewBag.Name = name;

            return View("Index", vm); 

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryServ.GetAllCategoriesAsync();
            ViewBag.Categories = categories;
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskVM taskVM)
        {
            taskVM.AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ModelState.Remove(nameof(taskVM.AppUserId));

            if (ModelState.IsValid)
            {
                var res = await _taskServs.CreateTaskAsync(taskVM);
                if (res)
                {
                    return RedirectToAction("index", "tasks");
                }
            }
            ModelState.AddModelError("", "Failed to create task.");
            var categories = await _categoryServ.GetAllCategoriesAsync();
            ViewBag.Categories = categories;
            return View(taskVM);
        }


        [AcceptVerbs("Get", "Post")]
        public IActionResult ValidateDueDate(DateTime DueDate, DateTime CreatedAt)
        {
            if (DueDate <= CreatedAt)
            {
                return Json("Due date must be later than Created At date.");
            }

            return Json(true);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var categories = await _categoryServ.GetAllCategoriesAsync();
            ViewBag.Categories = categories;
            TaskVM taskVM = await _taskServs.GetTaskByIdAsync(id);
            return View(taskVM);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(TaskVM taskVM)
        {
            taskVM.AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ModelState.Remove(nameof(taskVM.AppUserId));
            if (ModelState.IsValid)
            {
                var res = await _taskServs.UpdateTaskAsync(taskVM);
                if (res)
                {
                    return RedirectToAction("index", "tasks");
                }
            }
            ModelState.AddModelError("", "Failed to update task.");
            return View(taskVM);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _taskServs.DeleteTaskAsync(id);
            if (res)
            {
                return RedirectToAction("index", "tasks");
            }
            ModelState.AddModelError("", "Failed to delete task.");
            return RedirectToAction("index", "tasks");
        }
    }
}
