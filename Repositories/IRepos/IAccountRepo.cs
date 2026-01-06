using Tasky.Models;
using Tasky.VMS;
using Tasky.VMS.Account;

namespace Tasky.Repositories.IRepos
{
    public interface IAccountRepo
    {
        public Task<bool> CreateUserAsync(AppUser User,string Password);
        public Task<bool> LoginAsync(AppUser User,string Password, bool Presistent);
        public Task LogoutAsync();

        public Task<AppUser?> GetProfileAsync();
       
        public Task<bool> UpdateUserAsync(AppUser user);

        public Task<bool> ChangePasswordAsync(ChangePsVM psVM);

        public Task<bool> IsEmailInUseAsync(string email);
       



    }
}
