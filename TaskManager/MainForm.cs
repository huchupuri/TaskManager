using NLog;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Forms
{
    public partial class MainForm : Form
    {
        private readonly ITaskService _taskService;
        private readonly TaskEditForm _taskEditForm;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private TextBox txtNewTask;
        private ComboBox cbPriority;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnToggleComplete;
        private TextBox txtSearch;
        private ListView lvTasks;
        private Label lblStats;

        public MainForm(ITaskService taskService, TaskEditForm taskEditForm)
        {
            _taskService = taskService;
            _taskEditForm = taskEditForm;
            InitializeComponent();
            LoadTasksAsync();
        }

        private void InitializeComponent()
        {
            this.Text = "Управление задачами - NLog";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Search
            var lblSearch = new Label
            {
                Text = "Поиск:",
                Location = new Point(10, 10),
                Size = new Size(50, 23)
            };

            txtSearch = new TextBox
            {
                Location = new Point(70, 10),
                Size = new Size(200, 23)
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // New task input
            var lblNewTask = new Label
            {
                Text = "Новая задача:",
                Location = new Point(10, 45),
                Size = new Size(100, 23)
            };

            txtNewTask = new TextBox
            {
                Location = new Point(120, 45),
                Size = new Size(300, 23)
            };

            // Priority
            var lblPriority = new Label
            {
                Text = "Приоритет:",
                Location = new Point(430, 45),
                Size = new Size(70, 23)
            };

            cbPriority = new ComboBox
            {
                Location = new Point(510, 45),
                Size = new Size(100, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbPriority.Items.AddRange(new[] { "Низкий", "Средний", "Высокий" });
            cbPriority.SelectedIndex = 1; // Medium

            // Buttons
            btnAdd = new Button
            {
                Text = "Добавить",
                Location = new Point(620, 45),
                Size = new Size(80, 25)
            };
            btnAdd.Click += BtnAdd_Click;

            btnEdit = new Button
            {
                Text = "Изменить",
                Location = new Point(10, 80),
                Size = new Size(80, 25)
            };
            btnEdit.Click += BtnEdit_Click;

            btnDelete = new Button
            {
                Text = "Удалить",
                Location = new Point(100, 80),
                Size = new Size(80, 25)
            };
            btnDelete.Click += BtnDelete_Click;

            btnToggleComplete = new Button
            {
                Text = "Выполнено",
                Location = new Point(190, 80),
                Size = new Size(80, 25)
            };
            btnToggleComplete.Click += BtnToggleComplete_Click;

            // Tasks ListView
            lvTasks = new ListView
            {
                Location = new Point(10, 115),
                Size = new Size(760, 400),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                MultiSelect = false
            };

            lvTasks.Columns.Add("ID", 50);
            lvTasks.Columns.Add("Задача", 300);
            lvTasks.Columns.Add("Описание", 200);
            lvTasks.Columns.Add("Приоритет", 80);
            lvTasks.Columns.Add("Статус", 80);
            lvTasks.Columns.Add("Создано", 120);

            lvTasks.SelectedIndexChanged += LvTasks_SelectedIndexChanged;

            // Stats
            lblStats = new Label
            {
                Location = new Point(10, 525),
                Size = new Size(400, 23),
                Text = "Статистика: 0 задач, 0 выполнено"
            };

            // Add controls
            this.Controls.AddRange(new Control[]
            {
                lblSearch, txtSearch,
                lblNewTask, txtNewTask,
                lblPriority, cbPriority,
                btnAdd, btnEdit, btnDelete, btnToggleComplete,
                lvTasks, lblStats
            });

            UpdateButtonStates();
        }

        private async void BtnAdd_Click(object sender, EventArgs e)
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

                _logger.Info("Task added successfully from UI");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding task from UI");
                MessageBox.Show($"Ошибка при добавлении задачи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnEdit_Click(object sender, EventArgs e)
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
                _logger.Error(ex, "Error editing task from UI");
                MessageBox.Show($"Ошибка при редактировании задачи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            if (lvTasks.SelectedItems.Count == 0) return;

            try
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить эту задачу?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var selectedItem = lvTasks.SelectedItems[0];
                    var taskId = int.Parse(selectedItem.Text);

                    await _taskService.DeleteTaskAsync(taskId);
                    await LoadTasksAsync();

                    _logger.Info("Task deleted successfully from UI");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting task from UI");
                MessageBox.Show($"Ошибка при удалении задачи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnToggleComplete_Click(object sender, EventArgs e)
        {
            if (lvTasks.SelectedItems.Count == 0) return;

            try
            {
                var selectedItem = lvTasks.SelectedItems[0];
                var taskId = int.Parse(selectedItem.Text);

                await _taskService.ToggleTaskCompletionAsync(taskId);
                await LoadTasksAsync();

                _logger.Info("Task completion toggled from UI");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error toggling task completion from UI");
                MessageBox.Show($"Ошибка при изменении статуса задачи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            await LoadTasksAsync();
        }

        private void LvTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool hasSelection = lvTasks.SelectedItems.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
            btnToggleComplete.Enabled = hasSelection;
        }

        private async Task LoadTasksAsync()
        {
            try
            {
                IEnumerable<TaskItem> tasks;

                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    tasks = await _taskService.GetAllTasksAsync();
                }
                else
                {
                    tasks = await _taskService.SearchTasksAsync(txtSearch.Text);
                }

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

                await UpdateStatsAsync();
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading tasks in UI");
                MessageBox.Show($"Ошибка при загрузке задач: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task UpdateStatsAsync()
        {
            try
            {
                var totalCount = await _taskService.GetTaskCountAsync();
                var completedCount = await _taskService.GetCompletedTaskCountAsync();
                lblStats.Text = $"Статистика: {totalCount} задач, {completedCount} выполнено";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating stats in UI");
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
