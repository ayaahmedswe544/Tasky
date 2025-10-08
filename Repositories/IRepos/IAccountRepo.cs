using Tasky.Models;
using Tasky.VMS;

namespace Tasky.Repositories.IRepos
{
    public interface IAccountRepo
    {
        public Task<bool> CreateUserAsync(AppUser User,string Password);
        public Task<bool> LoginAsync(AppUser User,string Password, bool Presistent);
        public Task LogoutAsync();

        public Task<AppUser?> GetProfileAsync();
       
        public Task<bool> UpdateUserAsync(UpdateVM UserVM);

        public Task<bool> ChangePasswordAsync(ChangePsVM psVM);

        public Task<bool> IsEmailInUseAsync(string email);



    }
}
