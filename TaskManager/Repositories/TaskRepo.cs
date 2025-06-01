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
                _logger.Info("полчение задач из бд");
                return await _context.Tasks
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ошибка получения задач");
                throw;
            }
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            try
            {
                _logger.Info("получени езадачи с определнным ID", id);
                return await _context.Tasks.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ошибка при получени задачи с определнным ID", id);
                throw;
            }
        }

        public async Task<TaskItem> AddAsync(TaskItem task)
        {
            try
            {
                _logger.Info($"получение новой задачи", task.Title);
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                _logger.Info($"успешно добавлена задача", task.Id);
                return task;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"ошбика добавления задачи {task.Title}", task.Title);
                throw;
            }
        }

        public async Task<TaskItem> UpdateAsync(TaskItem task)
        {
            try
            {
                task.UpdatedAt = DateTime.Now;
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
                _logger.Info($"задача успешно обновлена: {task.Id}");
                return task;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"ОШИБКА добавления задачи: {task.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _logger.Info("удаление задачи", id);
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    _logger.Warn("задача не найдена", id);
                    return false;
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ошибка удаления задачи");
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
