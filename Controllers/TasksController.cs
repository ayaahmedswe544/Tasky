using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        private readonly IAccountServs _accountServs;
        public TasksController(ITaskServs taskServs, ICatServs catServs, IAccountServs accountServs)
        {
            _taskServs = taskServs;
            _categoryServ = catServs;
            _accountServs = accountServs;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            bool today = false,
    int? categoryId = null,
    PriorityLevel? priority = null,
    bool overdue = false,
    string? searchTerm = null,
    string? sortOrder = "asc",
    bool comp=false,
    int pageNumber = 1,
    int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var name = User.FindFirstValue(ClaimTypes.Name);
            var tasks = await _taskServs.GetAllTasksAsync(
                userId,
                today,
                categoryId,
                priority,
                overdue,
                searchTerm,
                sortOrder,
                pageNumber,
                pageSize,
                comp
            );

            var totalCount = await _taskServs.GetTotalTaskCountAsync(
                userId,
                today=false,
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
                Priority = priority,
                Overdue=overdue,
                Comp=comp
            };

            ViewBag.Categories = await _categoryServ.GetAllCategoriesAsync(userId, searchTerm = null, sortOrder = "asc", pageNumber = 1,pageSize=10);
            ViewBag.SelectedCategory = categoryId;
            ViewBag.SelectedPriority = priority;
            ViewBag.Overdue = overdue;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SortOrder = sortOrder;
            
           
            var UserProfile = await _accountServs.GetProfileAsync();

             ViewBag.FullName = UserProfile.FullName;
          

            return View("Index", vm); 

        }

        [HttpGet]
        public async Task<IActionResult> Create()

        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Categories = await _categoryServ.GetAllCategoriesAsync(userId, null, "asc", 1,10);

            return View(new TaskVM()
            {
                CreatedAt=DateTime.Today,
              DueDate=DateTime.Today
            });

        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskVM taskVM)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
            var categories = await _categoryServ.GetAllCategoriesAsync(userId, null, "asc", 1, 10);
            ViewBag.Categories = categories;
            return View(taskVM);
        }


        [AcceptVerbs("Get", "Post")]
        public IActionResult ValidateDueDate(DateTime DueDate, DateTime CreatedAt)
        {
            if (DueDate <CreatedAt)
            {
                return Json("Due date must be later than Created At date.");
            }

            return Json(true);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult ValidateDueDateCreate(DateTime DueDate)
        {
            if (DueDate < DateTime.Today)
            {
                return Json("Due date can't be in the past");
            }

            return Json(true);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)

        {
            
            EditTaskVm taskVM = await _taskServs.GetTaskByIdAsync(id);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(taskVM != null)
            {
                if (taskVM.AppUserId != userId)
                {
                    return RedirectToAction(nameof(Index));
                }
                else {             
                    var categories = await _categoryServ.GetAllCategoriesAsync(userId, null, "asc", 1, 10);
                     ViewBag.Categories = categories;
                return View(taskVM);
                }
              
            }
                
                return RedirectToAction(nameof(Index));
            

        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditTaskVm taskVM)
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

        [HttpPost]
        public async Task<IActionResult> ToggleCompleted(int id, bool completed)
        {
            var res = await _taskServs.ToggleTaskAsync(id, completed);
            if (res == 0) {
                return NotFound();
            }
            else
            {
                return Ok();
            }

        }

    }
}
