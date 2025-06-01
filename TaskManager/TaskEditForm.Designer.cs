using System;
using System.Drawing;
using System.Windows.Forms;

namespace TaskManagement.Forms
{
    public partial class TaskEditForm : Form
    {
        public TaskEditForm()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // логика сохранения
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            txtTitle = new TextBox();
            txtDescription = new TextBox();
            cbPriority = new ComboBox();
            chkCompleted = new CheckBox();
            btnSave = new Button();
            btnCancel = new Button();
            lblTitle = new Label();
            lblDescription = new Label();
            lblPriority = new Label();
            SuspendLayout();
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(100, 15);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(350, 27);
            txtTitle.TabIndex = 1;
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(100, 50);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.ScrollBars = ScrollBars.Vertical;
            txtDescription.Size = new Size(350, 80);
            txtDescription.TabIndex = 3;
            // 
            // cbPriority
            // 
            cbPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPriority.Items.AddRange(new object[] { "Низкий", "Средний", "Высокий" });
            cbPriority.Location = new Point(100, 145);
            cbPriority.Name = "cbPriority";
            cbPriority.Size = new Size(120, 28);
            cbPriority.TabIndex = 5;
            // 
            // chkCompleted
            // 
            chkCompleted.Location = new Point(100, 180);
            chkCompleted.Name = "chkCompleted";
            chkCompleted.Size = new Size(100, 23);
            chkCompleted.TabIndex = 6;
            chkCompleted.Text = "Выполнено";
            // 
            // btnSave
            // 
            btnSave.DialogResult = DialogResult.OK;
            btnSave.Location = new Point(280, 220);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(80, 30);
            btnSave.TabIndex = 7;
            btnSave.Text = "Сохранить";
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(370, 220);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(80, 30);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Отмена";
            // 
            // lblTitle
            // 
            lblTitle.Location = new Point(10, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(80, 23);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Название:";
            // 
            // lblDescription
            // 
            lblDescription.Location = new Point(10, 50);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(80, 23);
            lblDescription.TabIndex = 2;
            lblDescription.Text = "Описание:";
            // 
            // lblPriority
            // 
            lblPriority.Location = new Point(10, 145);
            lblPriority.Name = "lblPriority";
            lblPriority.Size = new Size(80, 23);
            lblPriority.TabIndex = 4;
            lblPriority.Text = "Приоритет:";
            // 
            // TaskEditForm
            // 
            AcceptButton = btnSave;
            CancelButton = btnCancel;
            ClientSize = new Size(482, 253);
            Controls.Add(lblTitle);
            Controls.Add(txtTitle);
            Controls.Add(lblDescription);
            Controls.Add(txtDescription);
            Controls.Add(lblPriority);
            Controls.Add(cbPriority);
            Controls.Add(chkCompleted);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TaskEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Редактирование задачи";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtTitle;
        private TextBox txtDescription;
        private ComboBox cbPriority;
        private CheckBox chkCompleted;
        private Button btnSave;
        private Button btnCancel;
        private Label lblTitle;
        private Label lblDescription;
        private Label lblPriority;
    }
}
