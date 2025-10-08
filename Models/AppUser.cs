using Microsoft.AspNetCore.Identity;

namespace Tasky.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
        public List<TaskItem>? Tasks { get; set; } = new List<TaskItem>();
    }
}
