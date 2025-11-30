using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Tasky.Models;

namespace Tasky.VMS.TaskVMs
{
    public class TaskVM
    {
        public int Id { get; set; }
        [Required]
        
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public bool IsCompleted { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Today;
        [DataType(DataType.Date)]
        [Remote(action: "ValidateDueDateCreate", controller: "Tasks", ErrorMessage = "Due date can't be in the past.")]
        public DateTime DueDate { get; set; } 
        [Required]
        public PriorityLevel Priority { get; set; }
        
        public string AppUserId { get; set; }
        
        public int? CategoryId { get; set; }
    }
}
