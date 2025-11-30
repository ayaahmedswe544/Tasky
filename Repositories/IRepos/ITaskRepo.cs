using Tasky.Models;

namespace Tasky.Repositories.IRepos
{
    public interface ITaskRepo
    {
        Task<bool> CreateTaskAsync(TaskItem task);

        Task<bool> UpdateTaskAsync(TaskItem task);

        Task<bool> DeleteTaskAsync(int id);
        Task<TaskItem?> GetTaskByIdAsync(int id);
        Task<IEnumerable<TaskItem>> GetAllTasksAsync(
            string userId,
            int? categoryId = null,
            PriorityLevel? priority = null,
            bool overdue = false,
            string? searchTerm = null,
            string? sortOrder = "asc",
            int pageNumber = 1,
            int pageSize = 10,
            bool comp = false);

        Task<int> GetTotalTaskCountAsync(
            string userId,
            int? categoryId = null,
            PriorityLevel? priority = null,
            bool overdue = false,
            string? searchTerm = null,
            bool comp = false);
    }
}
