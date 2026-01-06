using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tasky.Models;
using Tasky.Services.IServs;
using Tasky.VMS.Account;

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
                return RedirectToAction("dashboard","home" );
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


        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var updateVM = await _accountServs.GetProfileAsync() ?? new UpdateVM();
            var model = new ProfileVM
            {
                UpdateVM = updateVM,
                ChangePsVM = new ChangePsVM()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UpdateVM updateVM)
        {
            if (!ModelState.IsValid)
            {
                var model = new ProfileVM
                {
                    UpdateVM = updateVM,
                    ChangePsVM = new ChangePsVM()
                };
                return View("Profile", model);
            }

            var result = await _accountServs.UpdateUserAsync(updateVM);

            var profileModel = new ProfileVM
            {
                UpdateVM = await _accountServs.GetProfileAsync() ?? new UpdateVM(),
                ChangePsVM = new ChangePsVM()
            };

            if (!result)
            {
                ModelState.AddModelError("", "Update failed");
                return View("Profile", profileModel);
            }

            return RedirectToAction("Profile");
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                var vm = new ProfileVM
                {
                    UpdateVM = await _accountServs.GetProfileAsync() ?? new UpdateVM(),
                    ChangePsVM = model.ChangePsVM
                };
                return View("Profile", vm);
            }

            var result = await _accountServs.ChangePasswordAsync(model.ChangePsVM);

            if (!result)
            {
                ModelState.AddModelError("", "Password change failed.");
                var vm = new ProfileVM
                {
                    UpdateVM = await _accountServs.GetProfileAsync() ?? new UpdateVM(),
                    ChangePsVM = model.ChangePsVM
                };
                return View("Profile", vm);
            }

            return RedirectToAction("Profile");
        }
        public async Task<IActionResult> Logout()
        {
             await _accountServs.LogoutAsync();
            return RedirectToAction("Login");
        }

    }
}
