using Tasky.Models;
using Tasky.Repositories.IRepos;
using Tasky.Services.IServs;
using Tasky.VMS.TaskVMs;

namespace Tasky.Services.Servs
{
    public class TaskServs:ITaskServs
    {
        private readonly ITaskRepo _taskRepo;
        public TaskServs(ITaskRepo taskRepo)
        {
            _taskRepo = taskRepo;
        }

        public async Task<bool> CreateTaskAsync(TaskVM TaskVM)
        {
            TaskItem taskItem = new TaskItem
            {
                Title = TaskVM.Title,
                Description = TaskVM.Description,
                IsCompleted = TaskVM.IsCompleted,
                CreatedAt = TaskVM.CreatedAt,
                DueDate = TaskVM.DueDate,
                Priority = TaskVM.Priority,
                AppUserId = TaskVM.AppUserId,
                CategoryId = TaskVM.CategoryId
            };
            return await _taskRepo.CreateTaskAsync(taskItem);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            return await _taskRepo.DeleteTaskAsync(id);
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync(string userId, int? categoryId = null, PriorityLevel? priority = null, bool overdue = false, string? searchTerm = null, string? sortOrder = "asc", int pageNumber = 1, int pageSize = 10)
        {
            return await _taskRepo.GetAllTasksAsync(userId, categoryId, priority, overdue, searchTerm, sortOrder, pageNumber, pageSize);
        }

        public async Task<TaskVM?> GetTaskByIdAsync(int id)
        {
            TaskItem taskitem= await _taskRepo.GetTaskByIdAsync(id);
            TaskVM taskvm = new TaskVM();
            if (taskitem == null)
            {   
                return null;
            }
            taskvm.Id = taskitem.Id;
            taskvm.Title = taskitem.Title;
            taskvm.Description = taskitem.Description;
            taskvm.IsCompleted = taskitem.IsCompleted;
            taskvm.CreatedAt = taskitem.CreatedAt;
            taskvm.DueDate = taskitem.DueDate;
            taskvm.Priority = taskitem.Priority;
            taskvm.AppUserId = taskitem.AppUserId;
            taskvm.CategoryId = taskitem.CategoryId;
            return taskvm;
        }

        public async Task<int> GetTotalTaskCountAsync(string userId, int? categoryId = null, PriorityLevel? priority = null, bool overdue = false, string? searchTerm = null)
        {
            return await _taskRepo.GetTotalTaskCountAsync(userId, categoryId, priority, overdue, searchTerm);
        }

        public async Task<bool> UpdateTaskAsync(TaskVM TaskVM)
        {
            TaskItem Task=new TaskItem()
            {
                Id = TaskVM.Id,
                Title = TaskVM.Title,
                Description = TaskVM.Description,
                IsCompleted = TaskVM.IsCompleted,
                CreatedAt = TaskVM.CreatedAt,
                DueDate = TaskVM.DueDate,
                Priority = TaskVM.Priority,
                AppUserId = TaskVM.AppUserId,
                CategoryId = TaskVM.CategoryId
            };
            return await _taskRepo.UpdateTaskAsync(Task);
        }
    }
}
