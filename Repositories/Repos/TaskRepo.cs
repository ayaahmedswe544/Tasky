using Microsoft.EntityFrameworkCore;
using Tasky.Models;
using Tasky.Repositories.IRepos;

namespace Tasky.Repositories.Repos
{
    public class TaskRepo : ITaskRepo
    {
        private readonly AppDbContext _context;

        public TaskRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateTaskAsync(TaskItem task)
        {
            _context.TaskItems.Add(task);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateTaskAsync(TaskItem task)
        {
            _context.TaskItems.Update(task);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null) return false;

            _context.TaskItems.Remove(task);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            return await _context.TaskItems
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync(
            string userId,
            int? categoryId = null,
            PriorityLevel? priority = null,
            bool overdue = false,
            string? searchTerm = null,
            string? sortOrder = "asc",
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _context.TaskItems
                .Include(t => t.Category)
                .Where(t => t.AppUserId == userId)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(t => t.CategoryId == categoryId.Value);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            if (overdue)
                query = query.Where(t => t.DueDate < DateTime.Now && !t.IsCompleted);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lowerSearch = searchTerm.ToLower();
                query = query.Where(t =>
                    t.Title.ToLower().Contains(lowerSearch) ||
                    t.Description.ToLower().Contains(lowerSearch));
            }

            query = sortOrder?.ToLower() == "desc"
                ? query.OrderByDescending(t => t.DueDate)
                : query.OrderBy(t => t.DueDate);

            query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<int> GetTotalTaskCountAsync(
            string userId,
            int? categoryId = null,
            PriorityLevel? priority = null,
            bool overdue = false,
            string? searchTerm = null)
        {
            var query = _context.TaskItems.Where(t => t.AppUserId == userId);

            if (categoryId.HasValue)
                query = query.Where(t => t.CategoryId == categoryId.Value);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            if (overdue)
                query = query.Where(t => t.DueDate < DateTime.Now && !t.IsCompleted);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lowerSearch = searchTerm.ToLower();
                query = query.Where(t =>
                    t.Title.ToLower().Contains(lowerSearch) ||
                    t.Description.ToLower().Contains(lowerSearch));
            }

            return await query.CountAsync();
        }
    }
}
