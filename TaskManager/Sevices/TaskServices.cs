using NLog;
using TaskManagement.Models;
using TaskManagement.Repositories;

namespace TaskManagement.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            _logger.Info("получение всех задач");
            return await _taskRepository.GetAllAsync();
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            _logger.Info("получение задачи", id);
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<TaskItem> CreateTaskAsync(string title, string? description = null, TaskPriority priority = TaskPriority.Medium)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("заголовок не может быть пустым", nameof(title));
            }

            _logger.Info("создание нвоой задачи", title);

            var task = new TaskItem
            {
                Title = title.Trim(),
                Description = description?.Trim(),
                Priority = priority,
                CreatedAt = DateTime.Now
            };

            return await _taskRepository.AddAsync(task);
        }

        public async Task<TaskItem> UpdateTaskAsync(int id, string title, string? description = null, TaskPriority priority = TaskPriority.Medium, bool? isCompleted = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("заголовок не может быть пустым", nameof(title));
            }

            _logger.Info("обновить задачу", id);

            var existingTask = await _taskRepository.GetByIdAsync(id);
            if (existingTask == null)
            {
                throw new InvalidOperationException($"задача {id} не найдена");
            }

            existingTask.Title = title.Trim();
            existingTask.Description = description?.Trim();
            existingTask.Priority = priority;

            if (isCompleted.HasValue)
            {
                existingTask.IsCompleted = isCompleted.Value;
            }

            return await _taskRepository.UpdateAsync(existingTask);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            _logger.Info("удаление задачи", id);
            return await _taskRepository.DeleteAsync(id);
        }

        public async Task<bool> ToggleTaskCompletionAsync(int id)
        {
            _logger.Info("переключение статуса задачи", id);

            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return false;
            }

            task.IsCompleted = !task.IsCompleted;
            await _taskRepository.UpdateAsync(task);
            return true;
        }

        public async Task<IEnumerable<TaskItem>> SearchTasksAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllTasksAsync();
            }

            _logger.Info("поиск задачи", searchTerm);
            return await _taskRepository.SearchAsync(searchTerm);
        }

        public async Task<int> GetTaskCountAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return tasks.Count();
        }

        public async Task<int> GetCompletedTaskCountAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return tasks.Count(t => t.IsCompleted);
        }
    }
}
