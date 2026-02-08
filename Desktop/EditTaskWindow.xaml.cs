using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TaskFlow
{
    public partial class EditTaskWindow : Window
    {
        public TaskModel EditedTask { get; private set; }
        private TaskModel originalTask;

        public EditTaskWindow()
        {
            InitializeComponent();
            InitializeControls();
            Title = "Добавить новую задачу";
        }

        public EditTaskWindow(TaskModel taskToEdit) : this()
        {
            originalTask = taskToEdit;
            LoadTaskData(taskToEdit);
            Title = "Редактировать задачу";
        }

        private void InitializeControls()
        {
            datePicker.SelectedDate = DateTime.Today.AddDays(1);
            cmbTime.SelectedIndex = 9; // 17:00
            txtTitle.Focus();
        }

        private void LoadTaskData(TaskModel task)
        {
            txtTitle.Text = task.Title;
            txtDescription.Text = task.Description;
            datePicker.SelectedDate = task.DueDate;

            // Устанавливаем время
            var timeString = task.DueDate.ToString("HH:00");
            var item = cmbTime.Items.Cast<ComboBoxItem>()
                .FirstOrDefault(x => (string)x.Content == timeString);
            if (item != null)
            {
                cmbTime.SelectedItem = item;
            }

            chkImportant.IsChecked = task.IsImportant;
            txtNotes.Text = task.Notes;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Введите название задачи", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Получаем дату и время
            DateTime? selectedDate = datePicker.SelectedDate;
            if (!selectedDate.HasValue)
            {
                MessageBox.Show("Выберите дату", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var timeString = (cmbTime.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (string.IsNullOrEmpty(timeString) || !timeString.Contains(':'))
            {
                timeString = "17:00";
            }

            var timeParts = timeString.Split(':');
            if (timeParts.Length != 2 || !int.TryParse(timeParts[0], out int hours) || !int.TryParse(timeParts[1], out int minutes))
            {
                hours = 17;
                minutes = 0;
            }

            var dueDate = selectedDate.Value.Date.Add(new TimeSpan(hours, minutes, 0));

            EditedTask = new TaskModel
            {
                Title = txtTitle.Text.Trim(),
                Description = txtDescription.Text,
                DueDate = dueDate,
                IsImportant = chkImportant.IsChecked ?? false,
                Notes = txtNotes.Text,
                CreatedDate = originalTask?.CreatedDate ?? DateTime.Now
            };

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}