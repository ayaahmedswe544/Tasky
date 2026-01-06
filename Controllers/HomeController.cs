using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Tasky.Models;
using Tasky.Repositories.IRepos;
using Tasky.Services.IServs;
using Tasky.Services.Servs;
using Tasky.VMS;
using Tasky.VMS.TaskVMs;

namespace Tasky.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        IAccountServs _accountServs;
        ITaskServs _taskServs;
        public HomeController(IAccountServs accountServs,ITaskServs taskServs)
        {
            _accountServs = accountServs;
            _taskServs = taskServs;
        }
        private readonly List<string> Quotes = new()
    {"It does not matter how slowly you go as long as you do not stop.",
            "Success usually comes to those who are too busy to be looking for it",
           
        "Believe you can and you're halfway there.",
        "Success is not final, failure is not fatal: It is the courage to continue that counts.",
        "Don't watch the clock; do what it does. Keep going.",
        "Your limitation—it's only your imagination.",
        "Push yourself, because no one else is going to do it for you.",
        "Great things never come from comfort zones."
    };

      
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
            HomeVM homevm = new HomeVM();

            homevm.UserName = User.Identity.Name;
            var random = new Random();
            var quote = Quotes[random.Next(Quotes.Count)];
            ViewBag.Quote = quote;
            var userid=User.FindFirstValue(ClaimTypes.NameIdentifier);
            homevm.CompletedTasks = await _taskServs.GetTotalTaskCountAsync(
                userId,
                 false,
                null,
                null,
                false,
                null,
                true
            );


            homevm.TotalTasks = await _taskServs.GetTotalTaskCountAsync(
                userId,
                 false,
                null,
                null,
                false,
                null,
                false
            );
            homevm.todayTasks= await _taskServs.GetTotalTaskCountAsync(
                userid,
                true,
                null,
                null,
                false,
                null,
                false
            );
            homevm.PendingTasks=await _taskServs.GetTotalTaskCountAsync(
                userid,
                 false,
                null,
                null,
                true,
                null,
                false
            );
            homevm.OverdueTasks = await _taskServs.GetTotalTaskCountAsync(
                userid,
                 false,
                null,
                null,
                true,
                null,
                false
            );
            homevm.RecentTask = await _taskServs.GetAllTasksAsync(
                userid,
                 false,
                null,
                null,
                false,
                null,
                "desc",
                1,
                3,
                false
            );
            homevm.HighPriority = await _taskServs.GetTotalTaskCountAsync(
                userid,
                 false,
                null,
                PriorityLevel.High,
                false,
                null,
                false
            );
            ViewBag.Photo = UserProfile.ExistingProfilePhotoPath;
            ViewBag.TotalTasks = 200 ;
            ViewBag.CompletedTasks =  40;
            ViewBag.OverdueTasks = 10;
            ViewBag.TodayTasks = 40;
            ViewBag.HighPriority =100;

            return View("index",homevm);
        }

    }
}
