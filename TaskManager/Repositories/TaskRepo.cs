using Microsoft.EntityFrameworkCore;
using NLog;
using TaskManagement.Data;
using TaskManagement.Models;

namespace TaskManagement.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _context;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public TaskRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            try
            {
                _logger.Info("Retrieving all tasks from database");
                return await _context.Tasks
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving all tasks");
                throw;
            }
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            try
            {
                _logger.Info("Retrieving task with ID: {TaskId}", id);
                return await _context.Tasks.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving task with ID: {TaskId}", id);
                throw;
            }
        }

        public async Task<TaskItem> AddAsync(TaskItem task)
        {
            try
            {
                _logger.Info("Adding new task: {TaskTitle}", task.Title);
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                _logger.Info("Task added successfully with ID: {TaskId}", task.Id);
                return task;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding task: {TaskTitle}", task.Title);
                throw;
            }
        }

        public async Task<TaskItem> UpdateAsync(TaskItem task)
        {
            try
            {
                _logger.Info("Updating task with ID: {TaskId}", task.Id);
                task.UpdatedAt = DateTime.Now;
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
                _logger.Info("Task updated successfully: {TaskId}", task.Id);
                return task;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating task with ID: {TaskId}", task.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _logger.Info("Deleting task with ID: {TaskId}", id);
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    _logger.Warn("Task with ID {TaskId} not found for deletion", id);
                    return false;
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                _logger.Info("Task deleted successfully: {TaskId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting task with ID: {TaskId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<TaskItem>> SearchAsync(string searchTerm)
        {
            try
            {
                _logger.Info("Searching tasks with term: {SearchTerm}", searchTerm);
                return await _context.Tasks
                    .Where(t => t.Title.Contains(searchTerm) ||
                               (t.Description != null && t.Description.Contains(searchTerm)))
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error searching tasks with term: {SearchTerm}", searchTerm);
                throw;
            }
        }
    }
}
