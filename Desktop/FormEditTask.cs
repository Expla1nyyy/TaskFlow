using System;
using System.Windows.Forms;

namespace TaskFlow
{
    public partial class FormEditTask : Form
    {
        public TaskModel EditedTask { get; private set; }
        private TaskModel originalTask;

        public FormEditTask()
        {
            InitializeComponent();
            InitializeDateTimePickers();
            Text = "Добавить новую задачу";
        }

        public FormEditTask(TaskModel taskToEdit) : this()
        {
            originalTask = taskToEdit;
            LoadTaskData(taskToEdit);
            Text = "Редактировать задачу";
        }

        private void InitializeDateTimePickers()
        {
            dateTimePickerDate.Value = DateTime.Today.AddDays(1);
            dateTimePickerTime.Value = DateTime.Today.AddHours(17);
        }

        private void LoadTaskData(TaskModel task)
        {
            textBoxTitle.Text = task.Title;
            textBoxDescription.Text = task.Description;
            dateTimePickerDate.Value = task.DueDate;
            dateTimePickerTime.Value = DateTime.Today.Add(task.DueDate.TimeOfDay);
            checkBoxImportant.Checked = task.IsImportant;
            textBoxNotes.Text = task.Notes;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxTitle.Text))
            {
                MessageBox.Show("Введите название задачи", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EditedTask = new TaskModel
            {
                Title = textBoxTitle.Text,
                Description = textBoxDescription.Text,
                DueDate = dateTimePickerDate.Value.Date.Add(dateTimePickerTime.Value.TimeOfDay),
                IsImportant = checkBoxImportant.Checked,
                Notes = textBoxNotes.Text,
                CreatedDate = originalTask?.CreatedDate ?? DateTime.Now
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormEditTask_Load(object sender, EventArgs e)
        {
            textBoxTitle.Focus();
        }
    }
}