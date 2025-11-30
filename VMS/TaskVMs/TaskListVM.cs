using Tasky.Models;

namespace Tasky.VMS.TaskVMs
{
    public class TaskListVM
    {
        public IEnumerable<TaskItem> Tasks { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalTasks { get; set; }
        public string? SortOrder { get; set; }
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public bool Comp { get; set; }
        public bool Overdue { get; set; }

        public PriorityLevel? Priority { get; set; }
    }

}
