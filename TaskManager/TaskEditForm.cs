using NLog;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Forms
{
    public partial class TaskEditForm : Form
    {
        private readonly ITaskService _taskService;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private TaskItem? _currentTask;



        public TaskEditForm(ITaskService taskService)
        {
            _taskService = taskService;
            InitializeComponent();
        }



        public void SetTask(TaskItem task)
        {
            _currentTask = task;
            txtTitle.Text = task.Title;
            txtDescription.Text = task.Description ?? "";
            cbPriority.SelectedIndex = (int)task.Priority - 1;
            chkCompleted.Checked = task.IsCompleted;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Введите название задачи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_currentTask != null)
                {
                    var priority = (TaskPriority)(cbPriority.SelectedIndex + 1);
                    await _taskService.UpdateTaskAsync(
                        _currentTask.Id,
                        txtTitle.Text,
                        txtDescription.Text,
                        priority,
                        chkCompleted.Checked
                    );

                    _logger.Info("задача успешно обновлена", _currentTask.Id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении задачи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }

    }
}
