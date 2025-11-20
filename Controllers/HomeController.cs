using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasky.Models;
using Tasky.Services.IServs;
using Tasky.Services.Servs;

namespace Tasky.Controllers
{
    public class HomeController : Controller
    {
        IAccountServs _accountServs;
        public HomeController(IAccountServs accountServs)
        {
            _accountServs = accountServs;
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var UserProfile = await _accountServs.GetProfileAsync();
            if (userId == null) {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.FullName = UserProfile.FullName;
            }

            ViewBag.UserName = User.Identity.Name;
            //ViewBag.TotalTasks = await _taskServs.GetTotalTaskCountAsync(userId);
            //ViewBag.CompletedTasks = await _taskServs.GetCompletedTaskCountAsync(userId);
            //ViewBag.OverdueTasks = await _taskServs.GetOverdueTaskCountAsync(userId);
            //ViewBag.TodayTasks = await _taskServs.GetTodayTaskCountAsync(userId);
            //ViewBag.RecentTasks = await _taskServs.GetRecentTasksAsync(userId, 5);

            ViewBag.TotalTasks = 200 ;
            ViewBag.CompletedTasks =  40;
            ViewBag.OverdueTasks = 10;
            ViewBag.TodayTasks = 40;
            ViewBag.RecentTasks = new List<TaskItem>{
                new TaskItem
                {
                    Id = 2,
                    Title = "Grocery shopping",
                    Description = "Buy milk, eggs, and vegetables from the supermarket.",
                    DueDate = DateTime.Now.AddDays(1),
                    Priority = PriorityLevel.Medium,
                    IsCompleted = false,
                    CategoryId = 2,
                    Category = new Category { Id = 2, Name = "Personal" },
                    AppUserId = userId,
                    AppUser = new AppUser { Id = userId, UserName = "DemoUser" },
                    CreatedAt = DateTime.Now.AddDays(-2)
                }
            };
            ViewBag.HighPriority =100;

            return View("index");
        }

    }
}
