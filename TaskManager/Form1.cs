using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Forms
{
    public partial class Form1 : Form
    {
        private readonly ITaskService _taskService;
        private readonly TaskEditForm _taskEditForm;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();



        public Form1(ITaskService taskService, TaskEditForm taskEditForm)
        {
            _taskService = taskService;
            _taskEditForm = taskEditForm;
            InitializeComponent();
            LoadTasksAsync();
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            btnToggleComplete.Enabled = true;
        }


        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNewTask.Text))
                {
                    MessageBox.Show("Введите название задачи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var priority = (TaskPriority)(cbPriority.SelectedIndex + 1);
                await _taskService.CreateTaskAsync(txtNewTask.Text, null, priority);

                txtNewTask.Clear();
                cbPriority.SelectedIndex = 1;
                await LoadTasksAsync();

                _logger.Info("задача добавлена");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ошибка при добавлении задачи");
                MessageBox.Show($"Ошибка при добавлении задачи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvTasks.SelectedItems.Count == 0) return;

            try
            {
                var selectedItem = lvTasks.SelectedItems[0];
                var taskId = int.Parse(selectedItem.Text);
                var task = await _taskService.GetTaskByIdAsync(taskId);

                if (task != null)
                {
                    _taskEditForm.SetTask(task);
                    if (_taskEditForm.ShowDialog() == DialogResult.OK)
                    {
                        await LoadTasksAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ошибка при редактировании задачи");
                MessageBox.Show($"Ошибка при редактировании задачи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvTasks.SelectedItems.Count == 0) return;

            try
            {
                var selectedItem = lvTasks.SelectedItems[0];
                var taskId = int.Parse(selectedItem.Text);

                await _taskService.DeleteTaskAsync(taskId);
                await LoadTasksAsync();

                _logger.Info("задача успешно удалена");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ошибка при удалении задачи");
                MessageBox.Show($"Ошибка при удалении задачи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnToggleComplete_Click(object sender, EventArgs e)
        {
            if (lvTasks.SelectedItems.Count == 0) return;

            try
            {
                var selectedItem = lvTasks.SelectedItems[0];
                var taskId = int.Parse(selectedItem.Text);

                await _taskService.ToggleTaskCompletionAsync(taskId);
                await LoadTasksAsync();

                _logger.Info("изменение статусы задача");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ошибка при изменении статуса задачи: {ex.Message}");
                MessageBox.Show($"Ошибка при изменении статуса задачи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            await LoadTasksAsync();
        }

        private void UpdateButtonStates()
        {
            bool hasSelection = lvTasks.SelectedItems.Count > 0;
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            btnToggleComplete.Enabled = true;
        }

        private async Task LoadTasksAsync()
        {
            try
            {
                IEnumerable<TaskItem> tasks;

                tasks = await _taskService.GetAllTasksAsync();
                lvTasks.Items.Clear();

                foreach (var task in tasks)
                {
                    var item = new ListViewItem(task.Id.ToString());
                    item.SubItems.Add(task.Title);
                    item.SubItems.Add(task.Description ?? "");
                    item.SubItems.Add(GetPriorityText(task.Priority));
                    item.SubItems.Add(task.IsCompleted ? "Выполнено" : "В работе");
                    item.SubItems.Add(task.CreatedAt.ToString("dd.MM.yyyy HH:mm"));

                    if (task.IsCompleted)
                    {
                        item.ForeColor = Color.Gray;
                    }
                    else if (task.Priority == TaskPriority.High)
                    {
                        item.ForeColor = Color.Red;
                    }

                    lvTasks.Items.Add(item);
                }

                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ошибка загрузки задач");
                MessageBox.Show($"Ошибка при загрузке задач: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private string GetPriorityText(TaskPriority priority)
        {
            return priority switch
            {
                TaskPriority.Low => "Низкий",
                TaskPriority.Medium => "Средний",
                TaskPriority.High => "Высокий",
                _ => "Средний"
            };
        }
    }
}
