using Tasky.Models;
using Tasky.VMS;

namespace Tasky.Services.IServs
{
    public interface IAccountServs
    {
        public Task<bool> CreateUserAsync(RegisterVM UserVM);
        public Task<bool> LoginAsync(LoginVM UserVM);
        public Task LogoutAsync();
        public Task<AppUser?> GetProfileAsync();
        public Task<bool> UpdateUserAsync(UpdateVM UserVM);

        Task<bool> ChangePasswordAsync(ChangePsVM model);
        public Task<bool> IsEmailInUseAsync(string email);

    }
}
