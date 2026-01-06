using Tasky.Models;
using Tasky.VMS;
using Tasky.VMS.Account;

namespace Tasky.Services.IServs
{
    public interface IAccountServs
    {
        public Task<bool> CreateUserAsync(RegisterVM UserVM);
        public Task<bool> LoginAsync(LoginVM UserVM);
        public Task LogoutAsync();
        public Task<UpdateVM?> GetProfileAsync();
        public Task<bool> UpdateUserAsync(UpdateVM UserVM);
        public Task<bool> IsEmailInUseAsync(string email);
        public  Task<bool> ChangePasswordAsync(ChangePsVM model);

    }
}
