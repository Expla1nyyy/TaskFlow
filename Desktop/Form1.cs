using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            LoadSampleData();
        }

        private void InitializeData()
        {
            activeTasks = new BindingList<TaskModel>();
            completedTasks = new BindingList<TaskModel>();

            // Привязка данных к ListBox
            listBoxActiveTasks.DataSource = activeTasks;
            listBoxActiveTasks.DisplayMember = "Title";

            listBoxCompletedTasks.DataSource = completedTasks;
            listBoxCompletedTasks.DisplayMember = "Title";

            // Настройка внешнего вида
            UpdateViewButtons();
            UpdateTheme();
        }

        private void LoadSampleData()
        {
            // Активные задачи
            activeTasks.Add(new TaskModel
            {
                Title = "Подготовить отчет по проекту",
                Description = "Еженедельный отчет для руководства",
                DueDate = DateTime.Today.AddHours(17.5),
                CreatedDate = DateTime.Today.AddHours(10),
                IsImportant = true,
                Notes = "Не забыть про финансовые показатели"
            });

            activeTasks.Add(new TaskModel
            {
                Title = "Встреча с командой разработки",
                Description = "Обсуждение новых фич и багов",
                DueDate = DateTime.Today.AddHours(18.33),
                CreatedDate = DateTime.Today.AddHours(11),
                IsImportant = true,
                Notes = "Подготовить список вопросов"
            });

            activeTasks.Add(new TaskModel
            {
                Title = "Проверить почту",
                Description = "Ответить на важные письма",
                DueDate = DateTime.Today.AddHours(18.67),
                CreatedDate = DateTime.Today.AddHours(9),
                IsImportant = false,
                Notes = "Особое внимание письмам от клиентов"
            });

            // Выполненные задачи
            completedTasks.Add(new TaskModel
            {
                Title = "Позвонить клиенту",
                Description = "Обсудить условия договора",
                DueDate = DateTime.Today.AddHours(14),
                CreatedDate = DateTime.Today.AddHours(8),
                IsCompleted = true,
                IsImportant = false,
                Notes = "Клиент согласен на условия"
            });

            completedTasks.Add(new TaskModel
            {
                Title = "Отправить документы",
                Description = "Отправить скан документов в бухгалтерию",
                DueDate = DateTime.Today.AddHours(15),
                CreatedDate = DateTime.Today.AddHours(10),
                IsCompleted = true,
                IsImportant = true,
                Notes = "Документы отправлены, ждем подтверждения"
            });

            UpdateTaskCounters();
        }

        private void SetupEventHandlers()
        {
            // Текстовое поле для новой задачи
            textBoxNewTask.KeyDown += TextBoxNewTask_KeyDown;
            textBoxNewTask.Enter += TextBoxNewTask_Enter;
            textBoxNewTask.Leave += TextBoxNewTask_Leave;

            // Кнопки
            buttonAddTask.Click += ButtonAddTask_Click;
            buttonDelete.Click += ButtonDelete_Click;
            buttonComplete.Click += ButtonComplete_Click;
            buttonListView.Click += ButtonListView_Click;
            buttonCardsView.Click += ButtonCardsView_Click;

            // ListBox
            listBoxActiveTasks.SelectedIndexChanged += ListBoxTasks_SelectedIndexChanged;
            listBoxCompletedTasks.SelectedIndexChanged += ListBoxTasks_SelectedIndexChanged;

            // Меню
            menuItemAccount.Click += MenuItemAccount_Click;
            menuItemLightTheme.Click += MenuItemLightTheme_Click;
            menuItemDarkTheme.Click += MenuItemDarkTheme_Click;
            menuItemSettings.Click += MenuItemSettings_Click;

            // Датапикеры
            dateTimePickerDueDate.ValueChanged += DateTimePickerDueDate_ValueChanged;
            dateTimePickerDueTime.ValueChanged += DateTimePickerDueTime_ValueChanged;

            // Заметки
            textBoxNotes.TextChanged += TextBoxNotes_TextChanged;
        }

        #region Обработчики событий

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateDateNavigation();
            if (listBoxActiveTasks.Items.Count > 0)
            {
                listBoxActiveTasks.SelectedIndex = 0;
            }
        }

        private void TextBoxNewTask_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(textBoxNewTask.Text))
            {
                AddNewTask();
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void TextBoxNewTask_Enter(object sender, EventArgs e)
        {
            if (textBoxNewTask.Text == "введите текст задачи")
            {
                textBoxNewTask.Text = "";
                textBoxNewTask.ForeColor = isDarkTheme ? Color.White : Color.Black;
            }
        }

        private void TextBoxNewTask_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxNewTask.Text))
            {
                textBoxNewTask.Text = "введите текст задачи";
                textBoxNewTask.ForeColor = Color.Gray;
            }
        }

        private void ButtonAddTask_Click(object sender, EventArgs e)
        {
            AddNewTask();
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

            // Логика переключения вида
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

            // Логика переключения вида
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

        private void DateTimePickerDueDate_ValueChanged(object sender, EventArgs e)
        {
            if (selectedTask != null)
            {
                var newDate = dateTimePickerDueDate.Value.Date;
                var time = dateTimePickerDueTime.Value.TimeOfDay;
                selectedTask.DueDate = newDate.Add(time);
                UpdateTaskCounters();
            }
        }

        private void DateTimePickerDueTime_ValueChanged(object sender, EventArgs e)
        {
            if (selectedTask != null)
            {
                var date = dateTimePickerDueDate.Value.Date;
                var newTime = dateTimePickerDueTime.Value.TimeOfDay;
                selectedTask.DueDate = date.Add(newTime);
            }
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
                UpdateStatus($"Задача отмечена как {(selectedTask.IsImportant ? "важная" : "обычная")}");
            }
        }

        #endregion

        #region Методы работы с задачами

        private void AddNewTask()
        {
            var taskText = textBoxNewTask.Text.Trim();
            if (string.IsNullOrWhiteSpace(taskText) || taskText == "введите текст задачи")
            {
                return;
            }

            var newTask = new TaskModel
            {
                Title = taskText,
                Description = "Описание задачи",
                DueDate = DateTime.Today.AddDays(1).AddHours(17),
                CreatedDate = DateTime.Now,
                IsImportant = false,
                Notes = "Заметки к задаче"
            };

            activeTasks.Add(newTask);
            textBoxNewTask.Clear();
            textBoxNewTask.Focus();

            listBoxActiveTasks.SelectedItem = newTask;
            UpdateTaskCounters();
            UpdateTaskCards();

            UpdateStatus($"Задача добавлена: {taskText}");
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
                ClearTaskDetails();
                UpdateTaskCounters();
                UpdateTaskCards();

                UpdateStatus($"Задача удалена: {taskTitle}");
            }
        }

        private void CompleteSelectedTask()
        {
            if (selectedTask != null && activeTasks.Contains(selectedTask))
            {
                selectedTask.IsCompleted = true;
                activeTasks.Remove(selectedTask);
                completedTasks.Insert(0, selectedTask);

                ClearTaskDetails();
                UpdateTaskCounters();
                UpdateTaskCards();

                UpdateStatus($"Задача выполнена: {selectedTask.Title}");
            }
        }

        private void DisplayTaskDetails(TaskModel task)
        {
            if (task == null) return;

            // Основная информация
            labelTaskTitle.Text = task.Title;
            labelTaskDescription.Text = task.Description;
            labelDueTime.Text = $"выполнить до {task.DueTime}";
            labelCreatedTime.Text = $"создана в {task.CreatedTime}";

            // Дата и время
            dateTimePickerDueDate.Value = task.DueDate;
            dateTimePickerDueTime.Value = DateTime.Today.Add(task.DueDate.TimeOfDay);

            // Заметки
            textBoxNotes.Text = task.Notes;

            // Важность
            buttonImportant.Text = task.IsImportant ? "★ Важная" : "☆ Обычная";
            buttonImportant.BackColor = task.IsImportant ? Color.Gold : SystemColors.Control;

            // Обновить навигацию по датам
            UpdateDateNavigation(task.DueDate);
        }

        private void ClearTaskDetails()
        {
            labelTaskTitle.Text = "Текст задачи";
            labelTaskDescription.Text = "подтекст задачи";
            labelDueTime.Text = "выполнить до 17:30";
            labelCreatedTime.Text = "создана в 10:21";
            textBoxNotes.Text = "текст\nтекст";
            buttonImportant.Text = "☆ Обычная";
            buttonImportant.BackColor = SystemColors.Control;
        }

        private void UpdateTaskCounters()
        {
            var tomorrow = DateTime.Today.AddDays(1);
            var dayAfterTomorrow = DateTime.Today.AddDays(2);

            var tomorrowCount = activeTasks.Count(t => t.DueDate.Date == tomorrow);
            var dayAfterCount = activeTasks.Count(t => t.DueDate.Date == dayAfterTomorrow);
            var dayAfterImportant = activeTasks.Count(t => t.DueDate.Date == dayAfterTomorrow && t.IsImportant);

            labelTomorrowCount.Text = $"{tomorrowCount} задач";
            labelDayAfterCount.Text = $"{dayAfterCount} задач, {dayAfterImportant} важных";
            labelWednesdayCount.Text = $"задач: {activeTasks.Count}";
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
        }

        private Panel CreateTaskCard(TaskModel task)
        {
            var panel = new Panel
            {
                Width = panelTaskCards.Width - 20,
                Height = 80,
                BackColor = isDarkTheme ? Color.FromArgb(50, 50, 50) : Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(15)
            };

            var titleLabel = new Label
            {
                Text = task.Title,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = isDarkTheme ? Color.White : Color.Black,
                Location = new Point(10, 10),
                AutoSize = true
            };

            var timeLabel = new Label
            {
                Text = $"до {task.DueTime}",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(10, 35),
                AutoSize = true
            };

            var importantLabel = task.IsImportant ? new Label
            {
                Text = "★",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Gold,
                Location = new Point(panel.Width - 40, 10),
                AutoSize = true
            } : null;

            panel.Controls.Add(titleLabel);
            panel.Controls.Add(timeLabel);
            if (importantLabel != null) panel.Controls.Add(importantLabel);

            // Двойной щелчок для выбора задачи
            panel.DoubleClick += (s, e) =>
            {
                selectedTask = task;
                DisplayTaskDetails(task);
            };

            return panel;
        }

        #endregion

        #region Методы обновления UI

        private void UpdateDateNavigation(DateTime? selectedDate = null)
        {
            var dates = new[] { 19, 20, 21, 22, 23 }; // Дни января из макета
            var labels = new[] { labelDate19, labelDate20, labelDate21, labelDate22, labelDate23 };

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Text = $"{dates[i]} янв.";

                if (selectedDate.HasValue && selectedDate.Value.Day == dates[i])
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
                // Темная тема
                this.BackColor = Color.FromArgb(45, 45, 45);
                panelLeft.BackColor = Color.FromArgb(30, 30, 30);
                panelMain.BackColor = Color.FromArgb(50, 50, 50);
                panelRight.BackColor = Color.FromArgb(30, 30, 30);

                // Обновить цвета текста
                UpdateControlColors(this, Color.White);
                textBoxNewTask.BackColor = Color.FromArgb(64, 64, 64);
                textBoxNotes.BackColor = Color.FromArgb(64, 64, 64);
            }
            else
            {
                // Светлая тема
                this.BackColor = SystemColors.Control;
                panelLeft.BackColor = Color.FromArgb(245, 245, 245);
                panelMain.BackColor = Color.White;
                panelRight.BackColor = Color.FromArgb(245, 245, 245);

                // Обновить цвета текста
                UpdateControlColors(this, Color.Black);
                textBoxNewTask.BackColor = Color.White;
                textBoxNotes.BackColor = Color.White;
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

                if (ctrl is TextBox textBox && textBox != textBoxNewTask && textBox != textBoxNotes)
                {
                    textBox.ForeColor = textColor;
                    textBox.BackColor = control.BackColor;
                }

                UpdateControlColors(ctrl, textColor);
            }
        }

        private void UpdateStatus(string message)
        {
            toolStripStatusLabel.Text = message;
        }

        #endregion

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Сохранение данных
            SaveData();
            base.OnFormClosing(e);
        }

        private void SaveData()
        {
            // Здесь будет логика сохранения задач
            // Например, в файл или базу данных
            UpdateStatus("Данные сохранены");
        }
    }
}