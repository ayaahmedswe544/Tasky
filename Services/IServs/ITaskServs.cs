using Tasky.Models;
using Tasky.VMS.TaskVMs;

namespace Tasky.Repositories.IRepos
{
    public interface ITaskServs
    {
        Task<bool> CreateTaskAsync(TaskVM task);

        Task<bool> UpdateTaskAsync(EditTaskVm task);

        Task<bool> DeleteTaskAsync(int id);

        Task<EditTaskVm?> GetTaskByIdAsync(int id);
        Task<IEnumerable<TaskItem>> GetAllTasksAsync(
            string userId,
            bool today=false,
            int? categoryId = null,
            PriorityLevel? priority = null,
            bool overdue = false,
            string? searchTerm = null,
            string? sortOrder = "asc",
            int pageNumber = 1,
            int pageSize = 10,
            bool comp=false);
        Task<int> GetTotalTaskCountAsync(
            string userId,
            bool today = false,
            int? categoryId = null,
            PriorityLevel? priority = null,
            bool overdue = false,
            string? searchTerm = null,
            bool comp=false);
        Task<int> ToggleTaskAsync(int id, bool completed);
    }
}
