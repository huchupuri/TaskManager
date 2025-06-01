using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    }

    public enum TaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
}