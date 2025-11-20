using Microsoft.AspNetCore.Mvc.ModelBinding;
using Tasky.Models;
using Tasky.Repositories.IRepos;
using Tasky.Services.IServs;
using Tasky.VMS;

namespace Tasky.Services.Servs
{
    public class AccountServs:IAccountServs
    {
        private readonly IAccountRepo _AccountRepo;
        public AccountServs(IAccountRepo AccountRepo)
        {
            _AccountRepo = AccountRepo;
        }

        public async Task<bool> CreateUserAsync(RegisterVM UserVM)
        {
            AppUser user = new AppUser()
            {
                UserName = UserVM.Email,
                Email = UserVM.Email,
                FullName = UserVM.FullName,
            };

            return await _AccountRepo.CreateUserAsync(user, UserVM.Password);
        }

        public async Task<AppUser?> GetProfileAsync()
        {
            var user = await _AccountRepo.GetProfileAsync();
            return user;
        }

        public async Task<bool> LoginAsync(LoginVM UserVM)
        {
            AppUser user = new AppUser()
            {
                UserName = UserVM.Email,
                Email = UserVM.Email,
            };


            return await _AccountRepo.LoginAsync(user, UserVM.Password,UserVM.RememberMe);
        }

        public async Task LogoutAsync()
        {
           await  _AccountRepo.LogoutAsync();
        }


        public async Task<bool> ChangePasswordAsync(ChangePsVM model)
        {
            return await _AccountRepo.ChangePasswordAsync(model);
        }


        public Task<bool> UpdateUserAsync(UpdateVM UserVM)
        {
            return _AccountRepo.UpdateUserAsync(UserVM);
        }

        public Task<bool> IsEmailInUseAsync(string email)
        {
            return _AccountRepo.IsEmailInUseAsync(email);
        }
    }
}
