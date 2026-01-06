using Tasky.Models;
using Tasky.VMS.TaskVMs;

namespace Tasky.VMS
{
    public class HomeVM
    {
        public string UserName { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public int OverdueTasks { get; set; }
        public int HighPriority { get; set; }
        public int todayTasks { get; set; } 
        public IEnumerable<TaskItem> RecentTask { get; set; }


    }
}
