using Microsoft.Extensions.Logging;
using TaskManagement.Models;
using TaskManagement.Repositories;

namespace TaskManagement.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            _logger.LogInformation("Getting all tasks");
            return await _taskRepository.GetAllAsync();
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            _logger.LogInformation("Getting task by ID: {TaskId}", id);
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<TaskItem> CreateTaskAsync(string title, string? description = null, TaskPriority priority = TaskPriority.Medium)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Task title cannot be empty", nameof(title));
            }

            _logger.LogInformation("Creating new task: {TaskTitle}", title);

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
                throw new ArgumentException("Task title cannot be empty", nameof(title));
            }

            _logger.LogInformation("Updating task: {TaskId}", id);

            var existingTask = await _taskRepository.GetByIdAsync(id);
            if (existingTask == null)
            {
                throw new InvalidOperationException($"Task with ID {id} not found");
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
            _logger.LogInformation("Deleting task: {TaskId}", id);
            return await _taskRepository.DeleteAsync(id);
        }

        public async Task<bool> ToggleTaskCompletionAsync(int id)
        {
            _logger.LogInformation("Toggling task completion: {TaskId}", id);

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

            _logger.LogInformation("Searching tasks: {SearchTerm}", searchTerm);
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