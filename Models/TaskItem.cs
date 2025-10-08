
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasky.Models
{
    public enum PriorityLevel
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }


    }
}
