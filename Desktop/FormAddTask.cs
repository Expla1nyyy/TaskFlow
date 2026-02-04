using System;
using System.Windows.Forms;

namespace TaskFlow
{
    public partial class FormAddTask : Form
    {
        public TaskModel NewTask { get; private set; }

        public FormAddTask()
        {
            InitializeComponent();
            InitializeDateTimePickers();
        }

        private void InitializeDateTimePickers()
        {
            dateTimePickerDate.Value = DateTime.Today.AddDays(1);
            dateTimePickerTime.Value = DateTime.Today.AddHours(17);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxTitle.Text))
            {
                MessageBox.Show("Введите название задачи", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NewTask = new TaskModel
            {
                Title = textBoxTitle.Text,
                Description = textBoxDescription.Text,
                DueDate = dateTimePickerDate.Value.Date.Add(dateTimePickerTime.Value.TimeOfDay),
                IsImportant = checkBoxImportant.Checked,
                Notes = textBoxNotes.Text,
                CreatedDate = DateTime.Now
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormAddTask_Load(object sender, EventArgs e)
        {
            textBoxTitle.Focus();
        }
    }
}