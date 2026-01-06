using Microsoft.AspNetCore.Http.HttpResults;
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
            bool today=false,
            int? categoryId = null,
            PriorityLevel? priority = null,
            bool overdue = false,
            string? searchTerm = null,
            string? sortOrder = "asc",
            int pageNumber = 1,
            int pageSize = 10,
            bool comp = false)
        {
            var query = _context.TaskItems
                .Include(t => t.Category)
                .Where(t => t.AppUserId == userId)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(t => t.CategoryId == categoryId.Value);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);
            if (comp)
            {
                
                query = query.Where(t => t.IsCompleted ==true);
            }
               

            if (overdue)
                query = query.Where(t => t.DueDate < DateTime.Now && !t.IsCompleted);

            if(today)
            {
                var todayDate = DateTime.Today;
                query = query.Where(t => t.DueDate.Date == todayDate);
            }

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
            bool today = false,
            int? categoryId = null,
            PriorityLevel? priority = null,
            bool overdue = false,
            string? searchTerm = null,
            bool comp = false)
        {
            var query = _context.TaskItems.Where(t => t.AppUserId == userId);
            if (today)
            {
                var todayDate = DateTime.Today;
                query = query.Where(t => t.DueDate.Date == todayDate);
            }

            if (categoryId.HasValue)
                query = query.Where(t => t.CategoryId == categoryId.Value);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            if (overdue)
                query = query.Where(t => t.DueDate < DateTime.Now && !t.IsCompleted);
            if (comp)
            {

                query = query.Where(t => t.IsCompleted == true);
            }
            


            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lowerSearch = searchTerm.ToLower();
                query = query.Where(t =>
                    t.Title.ToLower().Contains(lowerSearch) ||
                    t.Description.ToLower().Contains(lowerSearch));
            }

            return await query.CountAsync();
        }

       public async Task<int> ToggleTaskAsync(int id, bool completed)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task==null)
            {
                return 0;
            }
            task.IsCompleted = completed;
            _context.TaskItems.Update(task);
            await _context.SaveChangesAsync();
            return 1;

        }
    }
}
