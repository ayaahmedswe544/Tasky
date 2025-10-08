using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tasky.Models;
using Tasky.Services.IServs;
using Tasky.VMS;

namespace Tasky.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountServs _accountServs;
        public AccountController( IAccountServs accaccountServs)
        {
            _accountServs = accaccountServs;

        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM UserVM)
        {
            if (!ModelState.IsValid)
            {
                return View(UserVM);
            }
            var result = await _accountServs.LoginAsync(UserVM);
            if (result)
            {
                var emo=User.FindFirstValue(ClaimTypes.Email);
                return Content($"user is logged in with Email{emo}");
            }
            else
            {
                ModelState.AddModelError("", "Invalid Login Attempt");
                return View(UserVM);
            }
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM UserVM)
        {
            if (!ModelState.IsValid)
            {
                return View(UserVM);
            }

            var result = await _accountServs.CreateUserAsync(UserVM);
            if (result)
            {
                return Content("user is created");
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(UserVM);
            }


        }
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var isInUse = await _accountServs.IsEmailInUseAsync(email);

            if (!isInUse)
            {
               
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use.");
            }
        }
        public IActionResult Logout()
        {
            _accountServs.LogoutAsync();
            return RedirectToAction("Login");
        }

    }
}
