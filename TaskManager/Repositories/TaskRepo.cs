using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Data;
using TaskManagement.Models;

namespace TaskManagement.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _context;
        private readonly ILogger<TaskRepository> _logger;

        public TaskRepository(TaskDbContext context, ILogger<TaskRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all tasks from database");
                return await _context.Tasks
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all tasks");
                throw;
            }
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving task with ID: {TaskId}", id);
                return await _context.Tasks.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving task with ID: {TaskId}", id);
                throw;
            }
        }

        public async Task<TaskItem> AddAsync(TaskItem task)
        {
            try
            {
                _logger.LogInformation("Adding new task: {TaskTitle}", task.Title);
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Task added successfully with ID: {TaskId}", task.Id);
                return task;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding task: {TaskTitle}", task.Title);
                throw;
            }
        }

        public async Task<TaskItem> UpdateAsync(TaskItem task)
        {
            try
            {
                _logger.LogInformation("Updating task with ID: {TaskId}", task.Id);
                task.UpdatedAt = DateTime.Now;
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Task updated successfully: {TaskId}", task.Id);
                return task;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task with ID: {TaskId}", task.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting task with ID: {TaskId}", id);
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    _logger.LogWarning("Task with ID {TaskId} not found for deletion", id);
                    return false;
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Task deleted successfully: {TaskId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task with ID: {TaskId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<TaskItem>> SearchAsync(string searchTerm)
        {
            try
            {
                _logger.LogInformation("Searching tasks with term: {SearchTerm}", searchTerm);
                return await _context.Tasks
                    .Where(t => t.Title.Contains(searchTerm) ||
                               (t.Description != null && t.Description.Contains(searchTerm)))
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching tasks with term: {SearchTerm}", searchTerm);
                throw;
            }
        }
    }
}