using TaskManagement.Models;

namespace TaskManagement.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        Task<TaskItem?> GetTaskByIdAsync(int id);
        Task<TaskItem> CreateTaskAsync(string title, string? description = null, TaskPriority priority = TaskPriority.Medium);
        Task<TaskItem> UpdateTaskAsync(int id, string title, string? description = null, TaskPriority priority = TaskPriority.Medium, bool? isCompleted = null);
        Task<bool> DeleteTaskAsync(int id);
        Task<bool> ToggleTaskCompletionAsync(int id);
        Task<IEnumerable<TaskItem>> SearchTasksAsync(string searchTerm);
        Task<int> GetTaskCountAsync();
        Task<int> GetCompletedTaskCountAsync();
    }
}