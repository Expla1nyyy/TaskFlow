using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TaskFlow
{
    public partial class Form1 : Form
    {
        private BindingList<TaskModel> activeTasks;
        private BindingList<TaskModel> completedTasks;
        private TaskModel selectedTask;
        private bool isDarkTheme = false;

        public Form1()
        {
            InitializeComponent();
            InitializeData();
            SetupEventHandlers();
            UpdateUIWithCurrentDate();
            ShowNoTaskSelected();
        }

        private void InitializeData()
        {
            activeTasks = new BindingList<TaskModel>();
            completedTasks = new BindingList<TaskModel>();

            listBoxActiveTasks.DataSource = activeTasks;
            listBoxActiveTasks.DisplayMember = "Title";

            listBoxCompletedTasks.DataSource = completedTasks;
            listBoxCompletedTasks.DisplayMember = "Title";

            UpdateViewButtons();
            UpdateTheme();
        }

        private void SetupEventHandlers()
        {
            buttonAddTask.Click += ButtonAddTask_Click;
            buttonDelete.Click += ButtonDelete_Click;
            buttonComplete.Click += ButtonComplete_Click;
            buttonEditTask.Click += ButtonEditTask_Click;
            buttonListView.Click += ButtonListView_Click;
            buttonCardsView.Click += ButtonCardsView_Click;

            listBoxActiveTasks.SelectedIndexChanged += ListBoxTasks_SelectedIndexChanged;
            listBoxCompletedTasks.SelectedIndexChanged += ListBoxTasks_SelectedIndexChanged;

            menuItemAccount.Click += MenuItemAccount_Click;
            menuItemLightTheme.Click += MenuItemLightTheme_Click;
            menuItemDarkTheme.Click += MenuItemDarkTheme_Click;
            menuItemSettings.Click += MenuItemSettings_Click;

            textBoxNotes.TextChanged += TextBoxNotes_TextChanged;
            buttonImportant.Click += ButtonImportant_Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateDateNavigation();
            UpdateTaskCounters();
            ShowNoTaskSelected();
        }

        private void ButtonAddTask_Click(object sender, EventArgs e)
        {
            using (var formEditTask = new FormEditTask())
            {
                if (formEditTask.ShowDialog() == DialogResult.OK)
                {
                    var newTask = formEditTask.EditedTask;
                    activeTasks.Add(newTask);
                    listBoxActiveTasks.SelectedItem = newTask;
                    UpdateTaskCounters();
                    UpdateTaskCards();
                    UpdateStatus($"Задача добавлена: {newTask.Title}");
                }
            }
        }

        private void ButtonEditTask_Click(object sender, EventArgs e)
        {
            if (selectedTask != null)
            {
                using (var formEditTask = new FormEditTask(selectedTask))
                {
                    if (formEditTask.ShowDialog() == DialogResult.OK)
                    {
                        var editedTask = formEditTask.EditedTask;

                        // Обновляем задачу в коллекции
                        if (activeTasks.Contains(selectedTask))
                        {
                            int index = activeTasks.IndexOf(selectedTask);
                            activeTasks[index] = editedTask;
                        }
                        else if (completedTasks.Contains(selectedTask))
                        {
                            int index = completedTasks.IndexOf(selectedTask);
                            completedTasks[index] = editedTask;
                        }

                        selectedTask = editedTask;
                        DisplayTaskDetails(editedTask);
                        UpdateTaskCounters();
                        UpdateTaskCards();
                        UpdateStatus($"Задача обновлена: {editedTask.Title}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите задачу для редактирования", "Редактирование",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            DeleteSelectedTask();
        }

        private void ButtonComplete_Click(object sender, EventArgs e)
        {
            CompleteSelectedTask();
        }

        private void ButtonListView_Click(object sender, EventArgs e)
        {
            buttonListView.BackColor = Color.FromArgb(33, 150, 243);
            buttonListView.ForeColor = Color.White;
            buttonCardsView.BackColor = isDarkTheme ? Color.FromArgb(64, 64, 64) : SystemColors.Control;
            buttonCardsView.ForeColor = isDarkTheme ? Color.White : Color.Black;

            panelTaskCards.Visible = false;
            listBoxActiveTasks.Visible = true;
            UpdateStatus("Вид изменен на список");
        }

        private void ButtonCardsView_Click(object sender, EventArgs e)
        {
            buttonCardsView.BackColor = Color.FromArgb(33, 150, 243);
            buttonCardsView.ForeColor = Color.White;
            buttonListView.BackColor = isDarkTheme ? Color.FromArgb(64, 64, 64) : SystemColors.Control;
            buttonListView.ForeColor = isDarkTheme ? Color.White : Color.Black;

            panelTaskCards.Visible = true;
            listBoxActiveTasks.Visible = false;
            UpdateTaskCards();
            UpdateStatus("Вид изменен на карточки");
        }

        private void ListBoxTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox?.SelectedItem is TaskModel task)
            {
                selectedTask = task;
                DisplayTaskDetails(task);
            }
            else
            {
                ShowNoTaskSelected();
            }
        }

        private void MenuItemAccount_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Настройки аккаунта\n(Функционал в разработке)",
                "Аккаунт", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MenuItemLightTheme_Click(object sender, EventArgs e)
        {
            isDarkTheme = false;
            menuItemLightTheme.Checked = true;
            menuItemDarkTheme.Checked = false;
            UpdateTheme();
            UpdateStatus("Тема изменена на светлую");
        }

        private void MenuItemDarkTheme_Click(object sender, EventArgs e)
        {
            isDarkTheme = true;
            menuItemDarkTheme.Checked = true;
            menuItemLightTheme.Checked = false;
            UpdateTheme();
            UpdateStatus("Тема изменена на темную");
        }

        private void MenuItemSettings_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Настройки приложения\n(Функционал в разработке)",
                "Настройки", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TextBoxNotes_TextChanged(object sender, EventArgs e)
        {
            if (selectedTask != null)
            {
                selectedTask.Notes = textBoxNotes.Text;
            }
        }

        private void ButtonImportant_Click(object sender, EventArgs e)
        {
            if (selectedTask != null)
            {
                selectedTask.IsImportant = !selectedTask.IsImportant;
                DisplayTaskDetails(selectedTask);
                UpdateTaskCards();
                UpdateStatus($"Задача отмечена как {(selectedTask.IsImportant ? "важная" : "обычная")}");
            }
        }

        private void CompleteSelectedTask()
        {
            if (selectedTask != null && activeTasks.Contains(selectedTask))
            {
                selectedTask.IsCompleted = true;
                activeTasks.Remove(selectedTask);
                completedTasks.Insert(0, selectedTask);

                ShowNoTaskSelected();
                UpdateTaskCounters();
                UpdateTaskCards();

                UpdateStatus($"Задача выполнена: {selectedTask.Title}");
            }
        }

        private void DeleteSelectedTask()
        {
            if (selectedTask != null)
            {
                var taskTitle = selectedTask.Title;

                if (activeTasks.Contains(selectedTask))
                {
                    activeTasks.Remove(selectedTask);
                }
                else if (completedTasks.Contains(selectedTask))
                {
                    completedTasks.Remove(selectedTask);
                }

                selectedTask = null;
                ShowNoTaskSelected();
                UpdateTaskCounters();
                UpdateTaskCards();

                UpdateStatus($"Задача удалена: {taskTitle}");
            }
        }

        private void DisplayTaskDetails(TaskModel task)
        {
            if (task == null)
            {
                ShowNoTaskSelected();
                return;
            }

            // Показать панель с деталями задачи
            panelNoTaskSelected.Visible = false;
            panelTaskDetails.Visible = true;

            string displayTitle = task.Title;
            if (displayTitle.Length > 50)
            {
                displayTitle = displayTitle.Substring(0, 47) + "...";
            }

            // Новый стиль отображения как в примере
            labelTaskTitle.Text = displayTitle.ToUpper(); // Большие буквы как заголовок
            labelTaskDescription.Text = task.Description;
            labelDueTime.Text = $"ВЫПОЛНИТЬ ДО {task.DueTime}";
            labelCreatedTime.Text = $"СОЗДАНА В {task.CreatedTime}";

            // Форматированная дата
            labelDateFull.Text = task.CreatedDate.ToString("d MMMM yyyy");
            labelTimeFull.Text = task.CreatedDate.ToString("HH:mm:ss");

            textBoxNotes.Text = task.Notes;

            buttonImportant.Text = task.IsImportant ? "★ Важная" : "☆ Обычная";
            buttonImportant.BackColor = task.IsImportant ? Color.Gold : SystemColors.Control;

            UpdateDateNavigation(task.DueDate);
        }

        private void ShowNoTaskSelected()
        {
            // Скрыть панель с деталями, показать сообщение
            panelTaskDetails.Visible = false;
            panelNoTaskSelected.Visible = true;

            // Обновить навигацию по датам (без выбранной даты)
            UpdateDateNavigation();
        }

        private void UpdateTaskCounters()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var dayAfterTomorrow = today.AddDays(2);

            // Задачи на завтра
            var tomorrowCount = activeTasks.Count(t => t.DueDate.Date == tomorrow);
            var tomorrowImportant = activeTasks.Count(t => t.DueDate.Date == tomorrow && t.IsImportant);
            labelTomorrowCount.Text = $"{tomorrowCount} задач" +
                (tomorrowImportant > 0 ? $", {tomorrowImportant} важных" : "");

            // Задачи на послезавтра
            var dayAfterCount = activeTasks.Count(t => t.DueDate.Date == dayAfterTomorrow);
            var dayAfterImportant = activeTasks.Count(t => t.DueDate.Date == dayAfterTomorrow && t.IsImportant);
            labelDayAfterCount.Text = $"{dayAfterCount} задач" +
                (dayAfterImportant > 0 ? $", {dayAfterImportant} важных" : "");
        }

        private void UpdateTaskCards()
        {
            panelTaskCards.Controls.Clear();

            int y = 10;
            foreach (var task in activeTasks)
            {
                var card = CreateTaskCard(task);
                card.Location = new Point(10, y);
                panelTaskCards.Controls.Add(card);
                y += card.Height + 10;
            }

            if (activeTasks.Count == 0)
            {
                var label = new Label
                {
                    Text = "Нет активных задач\nНажмите кнопку '+' чтобы добавить",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 11, FontStyle.Italic),
                    ForeColor = Color.Gray
                };
                panelTaskCards.Controls.Add(label);
            }
        }

        private Panel CreateTaskCard(TaskModel task)
        {
            var panel = new Panel
            {
                Width = panelTaskCards.Width - 25,
                Height = 100,
                BackColor = isDarkTheme ? Color.FromArgb(50, 50, 50) : Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(15),
                Tag = task
            };

            string title = task.Title;
            if (title.Length > 40)
            {
                title = title.Substring(0, 37) + "...";
            }

            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = isDarkTheme ? Color.White : Color.Black,
                Location = new Point(10, 10),
                AutoSize = true,
                MaximumSize = new Size(panel.Width - 100, 0)
            };

            var timeLabel = new Label
            {
                Text = $"до {task.DueTime}",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(10, 40),
                AutoSize = true
            };

            var completeButton = new Button
            {
                Text = "✓",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Green,
                BackColor = isDarkTheme ? Color.FromArgb(70, 70, 70) : Color.LightGreen,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(40, 40),
                Location = new Point(panel.Width - 160, 25),
                Tag = task
            };
            completeButton.FlatAppearance.BorderSize = 0;
            completeButton.Click += (s, e) =>
            {
                if (completeButton.Tag is TaskModel cardTask)
                {
                    selectedTask = cardTask;
                    CompleteSelectedTask();
                }
            };

            if (task.IsImportant)
            {
                var importantLabel = new Label
                {
                    Text = "★ Важная",
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.Gold,
                    Location = new Point(panel.Width - 110, 35),
                    AutoSize = true
                };
                panel.Controls.Add(importantLabel);
            }

            panel.Controls.Add(titleLabel);
            panel.Controls.Add(timeLabel);
            panel.Controls.Add(completeButton);

            panel.DoubleClick += (s, e) =>
            {
                selectedTask = task;
                DisplayTaskDetails(task);
            };

            return panel;
        }

        private void UpdateUIWithCurrentDate()
        {
            var today = DateTime.Today;
            labelTodayDate.Text = today.ToString("dddd, d MMMM");

            // Обновление только завтра и послезавтра
            labelTomorrow.Text = $"Завтра, {today.AddDays(1):d MMMM}";
            labelDayAfter.Text = $"Послезавтра, {today.AddDays(2):d MMMM}";
        }

        private void UpdateDateNavigation(DateTime? selectedDate = null)
        {
            var today = DateTime.Today;
            var dates = new[]
            {
                today.AddDays(-2),
                today.AddDays(-1),
                today,
                today.AddDays(1),
                today.AddDays(2)
            };

            var labels = new[] { labelDate19, labelDate20, labelDate21, labelDate22, labelDate23 };

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Text = dates[i].ToString("d MMM");

                if (selectedDate.HasValue && selectedDate.Value.Date == dates[i].Date)
                {
                    labels[i].ForeColor = Color.FromArgb(33, 150, 243);
                    labels[i].Font = new Font(labels[i].Font, FontStyle.Bold);
                }
                else if (dates[i].Date == today.Date)
                {
                    labels[i].ForeColor = Color.FromArgb(33, 150, 243);
                    labels[i].Font = new Font(labels[i].Font, FontStyle.Bold);
                }
                else
                {
                    labels[i].ForeColor = isDarkTheme ? Color.White : Color.Black;
                    labels[i].Font = new Font(labels[i].Font, FontStyle.Regular);
                }
            }
        }

        private void UpdateViewButtons()
        {
            buttonListView.BackColor = Color.FromArgb(33, 150, 243);
            buttonListView.ForeColor = Color.White;
            buttonCardsView.BackColor = isDarkTheme ? Color.FromArgb(64, 64, 64) : SystemColors.Control;
            buttonCardsView.ForeColor = isDarkTheme ? Color.White : Color.Black;
        }

        private void UpdateTheme()
        {
            if (isDarkTheme)
            {
                this.BackColor = Color.FromArgb(45, 45, 45);
                panelLeft.BackColor = Color.FromArgb(30, 30, 30);
                panelMain.BackColor = Color.FromArgb(50, 50, 50);
                panelRight.BackColor = Color.FromArgb(30, 30, 30);

                UpdateControlColors(this, Color.White);
                textBoxNotes.BackColor = Color.FromArgb(64, 64, 64);
                panelNoTaskSelected.BackColor = Color.FromArgb(30, 30, 30);
                panelTaskDetails.BackColor = Color.FromArgb(30, 30, 30);
            }
            else
            {
                this.BackColor = SystemColors.Control;
                panelLeft.BackColor = Color.FromArgb(245, 245, 245);
                panelMain.BackColor = Color.White;
                panelRight.BackColor = Color.FromArgb(245, 245, 245);

                UpdateControlColors(this, Color.Black);
                textBoxNotes.BackColor = Color.White;
                panelNoTaskSelected.BackColor = Color.FromArgb(245, 245, 245);
                panelTaskDetails.BackColor = Color.FromArgb(245, 245, 245);
            }

            UpdateViewButtons();
            UpdateTaskCards();
        }

        private void UpdateControlColors(Control control, Color textColor)
        {
            foreach (Control ctrl in control.Controls)
            {
                if (ctrl is Label || ctrl is Button || ctrl is GroupBox)
                {
                    ctrl.ForeColor = textColor;
                }

                if (ctrl is TextBox textBox && textBox != textBoxNotes)
                {
                    textBox.ForeColor = textColor;
                    textBox.BackColor = control.BackColor;
                }

                UpdateControlColors(ctrl, textColor);
            }
        }

        private void UpdateStatus(string message)
        {
            toolStripStatusLabel.Text = $"{DateTime.Now:HH:mm:ss} - {message}";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveData();
            base.OnFormClosing(e);
        }

        private void SaveData()
        {
            UpdateStatus("Данные сохранены");
        }
    }
}