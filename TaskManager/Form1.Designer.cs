namespace TaskManagement.Forms
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lvTasks = new ListView();
            cbPriority = new ComboBox();
            btnAdd = new Button();
            btnEdit = new Button();
            btnToggleComplete = new Button();
            btnDelete = new Button();
            txtNewTask = new TextBox();
            lblTask = new Label();
            SuspendLayout();
            // 
            // lvTasks
            // 
            lvTasks.Location = new Point(12, 134);
            lvTasks.Name = "lvTasks";
            lvTasks.Size = new Size(987, 387);
            lvTasks.TabIndex = 0;
            lvTasks.View = View.Details;
            lvTasks.FullRowSelect = true;
            lvTasks.GridLines = true;
            lvTasks.MultiSelect = false;
            lvTasks.UseCompatibleStateImageBehavior = false;
            // 
            // comboBox1
            // 
            cbPriority.FormattingEnabled = true;
            cbPriority.Items.AddRange(new object[] { "низкий", "средний", "высокий" });
            cbPriority.Location = new Point(516, 51);
            cbPriority.Name = "comboBox1";
            cbPriority.Size = new Size(151, 28);
            cbPriority.TabIndex = 1;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(54, 99);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(94, 29);
            btnAdd.TabIndex = 2;
            btnAdd.Text = "добавить";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(154, 99);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(94, 29);
            btnEdit.TabIndex = 3;
            btnEdit.Text = "редактировать";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnToggleComplete
            // 
            btnToggleComplete.Location = new Point(254, 99);
            btnToggleComplete.Name = "btnToggleComplete";
            btnToggleComplete.Size = new Size(108, 29);
            btnToggleComplete.TabIndex = 4;
            btnToggleComplete.Text = "выполнено";
            btnToggleComplete.UseVisualStyleBackColor = true;
            btnToggleComplete.Click += btnToggleComplete_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(368, 99);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(94, 29);
            btnDelete.TabIndex = 5;
            btnDelete.Text = "удалить";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // textBox1
            // 
            txtNewTask.Location = new Point(54, 51);
            txtNewTask.Name = "textBox1";
            txtNewTask.Size = new Size(384, 27);
            txtNewTask.TabIndex = 6;
            // 
            // lblTask
            // 
            lblTask.AutoSize = true;
            lblTask.Location = new Point(54, 18);
            lblTask.Name = "lblTask";
            lblTask.Size = new Size(126, 20);
            lblTask.TabIndex = 7;
            lblTask.Text = "создание задачи";
            lvTasks.Columns.Add("ID", 50);
            lvTasks.Columns.Add("Задача", 300);
            lvTasks.Columns.Add("Описание", 200);
            lvTasks.Columns.Add("Приоритет", 80);
            lvTasks.Columns.Add("Статус", 80);
            lvTasks.Columns.Add("Создано", 120);

            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1005, 523);
            Controls.Add(lblTask);
            Controls.Add(txtNewTask);
            Controls.Add(btnDelete);
            Controls.Add(btnToggleComplete);
            Controls.Add(btnEdit);
            Controls.Add(btnAdd);
            Controls.Add(cbPriority);
            Controls.Add(lvTasks);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView lvTasks;
        private ComboBox cbPriority;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnToggleComplete;
        private Button btnDelete;
        private TextBox txtNewTask;
        private Label lblTask;
    }
}