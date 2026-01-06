using Microsoft.AspNetCore.Mvc.ModelBinding;
using Tasky.Models;
using Tasky.Repositories.IRepos;
using Tasky.Repositories.Repos;
using Tasky.Services.IServs;
using Tasky.VMS;
using Tasky.VMS.Account;

namespace Tasky.Services.Servs
{
    public class AccountServs:IAccountServs
    {
        private readonly IAccountRepo _AccountRepo;
        private readonly IWebHostEnvironment _env;

        public AccountServs(IAccountRepo AccountRepo, IWebHostEnvironment env)
        {
            _AccountRepo = AccountRepo;
            _env = env;
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

        public async Task<UpdateVM?> GetProfileAsync()
        {
            var user = await _AccountRepo.GetProfileAsync();
            UpdateVM userVM= new UpdateVM();
            if (user == null)
                return null;
            userVM.FullName = user?.FullName;
           userVM.ExistingProfilePhotoPath = user?.ProfilePhotoPath;

            return userVM;
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


        public async Task<bool> UpdateUserAsync(UpdateVM model)
        {
            var user = await _AccountRepo.GetProfileAsync();
            if (user == null) return false;       
            if (!string.IsNullOrWhiteSpace(model.FullName))
                user.FullName = model.FullName;
            if (model.ProfilePhoto != null && model.ProfilePhoto.Length > 0)
            {
                try
                {
                    string folder = Path.Combine(_env.WebRootPath, "profilePhotos");
                    Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid() + Path.GetExtension(model.ProfilePhoto.FileName);
                    string path = Path.Combine(folder, fileName);

                    using var stream = new FileStream(path, FileMode.Create);
                    await model.ProfilePhoto.CopyToAsync(stream);

                    user.ProfilePhotoPath = "/profilePhotos/" + fileName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error uploading photo: {ex.Message}");
                    return false;
                }
            }

            return await _AccountRepo.UpdateUserAsync(user);
        }


        public Task<bool> IsEmailInUseAsync(string email)
        {
            return _AccountRepo.IsEmailInUseAsync(email);
        }
    }
}
