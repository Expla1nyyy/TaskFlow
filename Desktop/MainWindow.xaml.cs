using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace TaskFlow
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<TaskModel> activeTasks;
        private ObservableCollection<TaskModel> completedTasks;
        private TaskModel selectedTask;
        private bool isDarkTheme = false;

        public ObservableCollection<TaskModel> ActiveTasks
        {
            get => activeTasks;
            set
            {
                activeTasks = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TaskModel> CompletedTasks
        {
            get => completedTasks;
            set
            {
                completedTasks = value;
                OnPropertyChanged();
            }
        }

        public TaskModel SelectedTask
        {
            get => selectedTask;
            set
            {
                selectedTask = value;
                OnPropertyChanged();
                UpdateTaskDetails();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            InitializeData();
            UpdateUIWithCurrentDate();
            ShowNoTaskSelected();
        }

        private void InitializeData()
        {
            ActiveTasks = new ObservableCollection<TaskModel>();
            CompletedTasks = new ObservableCollection<TaskModel>();
        }

        private void UpdateUIWithCurrentDate()
        {
            var today = DateTime.Today;
            txtTodayDate.Text = today.ToString("dddd, d MMMM");

            // Обновление дат в левой панели
            txtTomorrow.Text = $"Завтра, {today.AddDays(1):d MMMM}";
            txtDayAfter.Text = $"Послезавтра, {today.AddDays(2):d MMMM}";

            // Обновление счетчиков
            UpdateTaskCounters();
        }

        private void UpdateTaskCounters()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var dayAfterTomorrow = today.AddDays(2);

            // Задачи на завтра
            var tomorrowCount = ActiveTasks.Count(t => t.DueDate.Date == tomorrow);
            var tomorrowImportant = ActiveTasks.Count(t => t.DueDate.Date == tomorrow && t.IsImportant);
            txtTomorrowCount.Text = $"{tomorrowCount} задач" +
                (tomorrowImportant > 0 ? $", {tomorrowImportant} важных" : "");

            // Задачи на послезавтра
            var dayAfterCount = ActiveTasks.Count(t => t.DueDate.Date == dayAfterTomorrow);
            var dayAfterImportant = ActiveTasks.Count(t => t.DueDate.Date == dayAfterTomorrow && t.IsImportant);
            txtDayAfterCount.Text = $"{dayAfterCount} задач" +
                (dayAfterImportant > 0 ? $", {dayAfterImportant} важных" : "");
        }

        private void UpdateTaskDetails()
        {
            if (SelectedTask == null)
            {
                ShowNoTaskSelected();
                return;
            }

            // Показать панель с деталями
            panelNoTaskSelected.Visibility = Visibility.Collapsed;
            panelTaskDetails.Visibility = Visibility.Visible;

            // Обновить информацию
            txtTaskTitle.Text = SelectedTask.Title.ToUpper();
            txtTaskDescription.Text = SelectedTask.Description;
            txtDueTime.Text = $"Выполнить до {SelectedTask.DueTime}";
            txtCreatedTime.Text = $"Создана в {SelectedTask.CreatedTime}";
            txtDateFull.Text = SelectedTask.CreatedDate.ToString("d MMMM yyyy");
            txtTimeFull.Text = SelectedTask.CreatedDate.ToString("HH:mm:ss");
            txtNotes.Text = SelectedTask.Notes;
            btnImportant.Content = SelectedTask.IsImportant ? "★ Важная" : "☆ Обычная";
        }

        private void ShowNoTaskSelected()
        {
            panelTaskDetails.Visibility = Visibility.Collapsed;
            panelNoTaskSelected.Visibility = Visibility.Visible;
        }

        #region Обработчики событий

        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new EditTaskWindow();
            if (editWindow.ShowDialog() == true)
            {
                var newTask = editWindow.EditedTask;
                ActiveTasks.Add(newTask);
                SelectedTask = newTask;
                UpdateTaskCounters();
            }
        }

        private void BtnEditTask_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask == null)
            {
                MessageBox.Show("Выберите задачу для редактирования", "Редактирование",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            EditTask(SelectedTask);
        }

        private void BtnEditTaskFromCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is TaskModel task)
            {
                EditTask(task);
            }
        }

        private void EditTask(TaskModel task)
        {
            var editWindow = new EditTaskWindow(task);
            if (editWindow.ShowDialog() == true)
            {
                var editedTask = editWindow.EditedTask;

                // Обновляем задачу в коллекции
                if (ActiveTasks.Contains(task))
                {
                    int index = ActiveTasks.IndexOf(task);
                    ActiveTasks[index] = editedTask;
                }
                else if (CompletedTasks.Contains(task))
                {
                    int index = CompletedTasks.IndexOf(task);
                    CompletedTasks[index] = editedTask;
                }

                if (SelectedTask == task)
                {
                    SelectedTask = editedTask;
                }
                UpdateTaskCounters();
            }
        }

        private void BtnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask == null) return;

            if (MessageBox.Show($"Удалить задачу '{SelectedTask.Title}'?", "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (ActiveTasks.Contains(SelectedTask))
                    ActiveTasks.Remove(SelectedTask);
                else if (CompletedTasks.Contains(SelectedTask))
                    CompletedTasks.Remove(SelectedTask);

                SelectedTask = null;
                ShowNoTaskSelected();
                UpdateTaskCounters();
            }
        }

        private void BtnCompleteTask_Click(object sender, RoutedEventArgs e)
        {
            CompleteTask(SelectedTask);
        }

        private void BtnCompleteTaskFromCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is TaskModel task)
            {
                CompleteTask(task);
            }
        }

        private void CompleteTask(TaskModel task)
        {
            if (task != null && ActiveTasks.Contains(task))
            {
                task.IsCompleted = true;
                task.CompletionDate = DateTime.Now;
                ActiveTasks.Remove(task);
                CompletedTasks.Insert(0, task);

                if (SelectedTask == task)
                {
                    SelectedTask = null;
                    ShowNoTaskSelected();
                }
                UpdateTaskCounters();
            }
        }

        private void BtnImportant_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask != null)
            {
                SelectedTask.IsImportant = !SelectedTask.IsImportant;
                UpdateTaskDetails();
            }
        }

        private void BtnListView_Click(object sender, RoutedEventArgs e)
        {
            btnListView.Style = (Style)FindResource("SelectedButtonStyle");
            btnCardsView.Style = (Style)FindResource("ButtonStyle");
            listTasks.Visibility = Visibility.Visible;
            cardsTasks.Visibility = Visibility.Collapsed;
        }

        private void BtnCardsView_Click(object sender, RoutedEventArgs e)
        {
            btnCardsView.Style = (Style)FindResource("SelectedButtonStyle");
            btnListView.Style = (Style)FindResource("ButtonStyle");
            listTasks.Visibility = Visibility.Collapsed;
            cardsTasks.Visibility = Visibility.Visible;
        }

        private void TaskCard_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is TaskModel task)
            {
                // Не выбираем задачу, если кликнули по кнопке
                if (e.OriginalSource is Button || e.OriginalSource is System.Windows.Controls.TextBlock)
                    return;

                SelectedTask = task;
            }
        }

        private void TxtNotes_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (SelectedTask != null)
            {
                SelectedTask.Notes = txtNotes.Text;
            }
        }

        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Настройки аккаунта\n(Функционал в разработке)",
                "Аккаунт", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnTheme_Click(object sender, RoutedEventArgs e)
        {
            isDarkTheme = !isDarkTheme;

            if (isDarkTheme)
            {
                // Темная тема
                Resources["BackgroundBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(45, 45, 45));
                Resources["CardBackground"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 30, 30));
                Resources["TextColor"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
                Resources["SecondaryText"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(200, 200, 200));
                Resources["BorderColor"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(70, 70, 70));
            }
            else
            {
                // Светлая тема
                Resources["BackgroundBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(245, 245, 245));
                Resources["CardBackground"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
                Resources["TextColor"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(51, 51, 51));
                Resources["SecondaryText"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(102, 102, 102));
                Resources["BorderColor"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(224, 224, 224));
            }
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Настройки приложения\n(Функционал в разработке)",
                "Настройки", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    // Конвертеры для стилей
    public class ImportantStyleConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is bool isImportant && isImportant)
                {
                    if (Application.Current.TryFindResource("ImportantTaskCardStyle") is Style importantStyle)
                    {
                        return importantStyle;
                    }
                }

                if (Application.Current.TryFindResource("TaskCardStyle") is Style normalStyle)
                {
                    return normalStyle;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanToVisibilityConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value is bool b && b) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}