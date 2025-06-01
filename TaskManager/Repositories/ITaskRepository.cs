using TaskManagement.Models;

namespace TaskManagement.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(int id);
        Task<TaskItem> AddAsync(TaskItem task);
        Task<TaskItem> UpdateAsync(TaskItem task);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TaskItem>> SearchAsync(string searchTerm);
    }
}