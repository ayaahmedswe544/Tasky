using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Tasky.Models;

namespace Tasky.VMS.TaskVMs
{
    public class EditTaskVm
    {

     
            public int Id { get; set; }
            [Required]

            public string Title { get; set; }

            [Required]
            public string Description { get; set; }

            public bool IsCompleted { get; set; }
            [DataType(DataType.Date)]
            [Required]
            public DateTime CreatedAt { get; set; } 
            [DataType(DataType.Date)]
            [Required]
            [Remote(action: "ValidateDueDate", controller: "Tasks", AdditionalFields = nameof(CreatedAt), ErrorMessage = "Due date must be later than Created At date.")]
            public DateTime DueDate { get; set; }
            [Required]
            public PriorityLevel Priority { get; set; }

            public string AppUserId { get; set; }

            public int? CategoryId { get; set; }
        
    

     }
}
