using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Tasky.Models;
using Tasky.Repositories.IRepos;
using Tasky.VMS;
using Tasky.VMS.Account;

namespace Tasky.Repositories.Repos
{
    public class AccountRepo : IAccountRepo
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public AccountRepo(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _env = env;
        }

     public async Task<bool> CreateUserAsync(AppUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);


            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                }
            }

            return result.Succeeded;
        }

        public async Task<AppUser?> GetProfileAsync()
        {
            var userId = _signInManager.Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return null;

            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        public async Task<bool> LoginAsync(AppUser user, string password, bool Presistent)
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, Presistent, false);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    Console.WriteLine("User account is locked out.");
                else if (result.IsNotAllowed)
                    Console.WriteLine("Login not allowed. Email might not be confirmed.");
                else
                    Console.WriteLine("Invalid login attempt.");
            }

            return result.Succeeded;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> IsEmailInUseAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;
            else
                return true;
        }
        public async Task<bool> UpdateUserAsync(AppUser user)
        {
            if (user == null)
                return false;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error updating user: {error.Description}");
                }
            }

            return result.Succeeded;
        }

        public async Task<bool> ChangePasswordAsync(ChangePsVM model)
        {
            var email = _signInManager.Context.User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return false;

            var result = await _userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword);

            return result.Succeeded;
        }


    }
}