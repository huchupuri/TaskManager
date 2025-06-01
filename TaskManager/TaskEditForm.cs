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

        private TextBox txtTitle;
        private TextBox txtDescription;
        private ComboBox cbPriority;
        private CheckBox chkCompleted;
        private Button btnSave;
        private Button btnCancel;

        public TaskEditForm(ITaskService taskService)
        {
            _taskService = taskService;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Редактирование задачи";
            this.Size = new Size(500, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Title
            var lblTitle = new Label
            {
                Text = "Название:",
                Location = new Point(10, 15),
                Size = new Size(80, 23)
            };

            txtTitle = new TextBox
            {
                Location = new Point(100, 15),
                Size = new Size(350, 23)
            };

            // Description
            var lblDescription = new Label
            {
                Text = "Описание:",
                Location = new Point(10, 50),
                Size = new Size(80, 23)
            };

            txtDescription = new TextBox
            {
                Location = new Point(100, 50),
                Size = new Size(350, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            // Priority
            var lblPriority = new Label
            {
                Text = "Приоритет:",
                Location = new Point(10, 145),
                Size = new Size(80, 23)
            };

            cbPriority = new ComboBox
            {
                Location = new Point(100, 145),
                Size = new Size(120, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbPriority.Items.AddRange(new[] { "Низкий", "Средний", "Высокий" });

            // Completed
            chkCompleted = new CheckBox
            {
                Text = "Выполнено",
                Location = new Point(100, 180),
                Size = new Size(100, 23)
            };

            // Buttons
            btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(280, 220),
                Size = new Size(80, 30),
                DialogResult = DialogResult.OK
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(370, 220),
                Size = new Size(80, 30),
                DialogResult = DialogResult.Cancel
            };

            // Add controls
            this.Controls.AddRange(new Control[]
            {
                lblTitle, txtTitle,
                lblDescription, txtDescription,
                lblPriority, cbPriority,
                chkCompleted,
                btnSave, btnCancel
            });

            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }

        public void SetTask(TaskItem task)
        {
            _currentTask = task;
            txtTitle.Text = task.Title;
            txtDescription.Text = task.Description ?? "";
            cbPriority.SelectedIndex = (int)task.Priority - 1;
            chkCompleted.Checked = task.IsCompleted;
        }

        private async void BtnSave_Click(object sender, EventArgs e)
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

                    _logger.Info("Task updated successfully from edit form: {TaskId}", _currentTask.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error saving task from edit form");
                MessageBox.Show($"Ошибка при сохранении задачи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }
    }
}
