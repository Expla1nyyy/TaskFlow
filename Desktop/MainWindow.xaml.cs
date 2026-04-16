using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace TaskFlow
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<TaskModel> activeTasks;
        private ObservableCollection<TaskModel> completedTasks;
        private TaskModel selectedTask;
        private AppSettings settings;
        private ApiService apiService;
        private DispatcherTimer autoSyncTimer;
        private bool isSyncing = false;

        private static readonly string TasksFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "TaskFlow", "tasks.json");

        public ObservableCollection<TaskModel> ActiveTasks
        {
            get => activeTasks ?? (activeTasks = new ObservableCollection<TaskModel>());
            set
            {
                activeTasks = value ?? new ObservableCollection<TaskModel>();
                OnPropertyChanged();
                SaveTasks();
            }
        }

        public ObservableCollection<TaskModel> CompletedTasks
        {
            get => completedTasks ?? (completedTasks = new ObservableCollection<TaskModel>());
            set
            {
                completedTasks = value ?? new ObservableCollection<TaskModel>();
                OnPropertyChanged();
                SaveTasks();
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

            try
            {
                settings = AppSettings.Load();

                apiService = new ApiService(settings.ServerUrl ?? "http://localhost:8000");
                if (!string.IsNullOrEmpty(settings.Token))
                {
                    apiService.SetToken(settings.Token);
                }

                ApplyTheme(settings.IsDarkTheme);

                LoadTasks();

                DataContext = this;
                UpdateUIWithCurrentDate();
                ShowNoTaskSelected();

                StartAutoSync();

                if (!string.IsNullOrEmpty(settings.Token))
                {
                    Dispatcher.BeginInvoke(new Action(async () => await SyncWithServer(false)));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка инициализации: {ex.Message}");
                ActiveTasks = new ObservableCollection<TaskModel>();
                CompletedTasks = new ObservableCollection<TaskModel>();
                DataContext = this;
                UpdateUIWithCurrentDate();
                ShowNoTaskSelected();
            }
        }

        private void StartAutoSync()
        {
            autoSyncTimer = new DispatcherTimer();
            autoSyncTimer.Interval = TimeSpan.FromMinutes(5);
            autoSyncTimer.Tick += async (s, e) =>
            {
                if (!string.IsNullOrEmpty(settings.Token) && !isSyncing)
                {
                    await SyncWithServer(false);
                }
            };
            autoSyncTimer.Start();
        }

        private void ApplyTheme(bool isDark)
        {
            var resources = Application.Current.Resources;

            if (isDark)
            {
                resources["BackgroundBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(18, 18, 18));
                resources["CardBackground"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 30, 30));
                resources["TextColor"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
                resources["SecondaryText"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(180, 180, 180));
                resources["BorderColor"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(60, 60, 60));
                if (btnTheme != null) btnTheme.Content = "☀️ светлая";
            }
            else
            {
                resources["BackgroundBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(245, 245, 245));
                resources["CardBackground"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
                resources["TextColor"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(51, 51, 51));
                resources["SecondaryText"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(102, 102, 102));
                resources["BorderColor"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(224, 224, 224));
                if (btnTheme != null) btnTheme.Content = "🌙 темная";
            }
        }

        private void LoadTasks()
        {
            try
            {
                if (File.Exists(TasksFile))
                {
                    string json = File.ReadAllText(TasksFile);

                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var tasks = JsonSerializer.Deserialize<ObservableCollection<TaskModel>>(json, options);

                        if (tasks != null)
                        {
                            var validTasks = tasks.Where(t => t != null).ToList();

                            ActiveTasks = new ObservableCollection<TaskModel>(
                                validTasks.Where(t => !t.IsCompleted));
                            CompletedTasks = new ObservableCollection<TaskModel>(
                                validTasks.Where(t => t.IsCompleted));
                        }
                        else
                        {
                            ActiveTasks = new ObservableCollection<TaskModel>();
                            CompletedTasks = new ObservableCollection<TaskModel>();
                        }
                    }
                    else
                    {
                        ActiveTasks = new ObservableCollection<TaskModel>();
                        CompletedTasks = new ObservableCollection<TaskModel>();
                    }
                }
                else
                {
                    ActiveTasks = new ObservableCollection<TaskModel>();
                    CompletedTasks = new ObservableCollection<TaskModel>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки задач: {ex.Message}");
                ActiveTasks = new ObservableCollection<TaskModel>();
                CompletedTasks = new ObservableCollection<TaskModel>();

                try
                {
                    if (File.Exists(TasksFile))
                    {
                        File.Delete(TasksFile);
                    }
                }
                catch { }
            }
        }

        private void SaveTasks()
        {
            try
            {
                var activeTasksCopy = ActiveTasks?.Where(t => t != null).ToList() ?? new List<TaskModel>();
                var completedTasksCopy = CompletedTasks?.Where(t => t != null).ToList() ?? new List<TaskModel>();

                var allTasks = activeTasksCopy.Concat(completedTasksCopy).ToList();

                string directory = Path.GetDirectoryName(TasksFile);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                string json = JsonSerializer.Serialize(allTasks, options);
                File.WriteAllText(TasksFile, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка сохранения: {ex.Message}");
            }
        }

        public List<TaskModel> GetAllTasks()
        {
            var allTasks = new List<TaskModel>();
            if (ActiveTasks != null)
                allTasks.AddRange(ActiveTasks.Where(t => t != null));
            if (CompletedTasks != null)
                allTasks.AddRange(CompletedTasks.Where(t => t != null));
            return allTasks;
        }

        public async Task<bool> SyncWithServer(bool showMessages = true)
        {
            if (isSyncing) return false;

            try
            {
                isSyncing = true;

                if (string.IsNullOrEmpty(settings.Token))
                {
                    if (showMessages)
                        MessageBox.Show("Сначала выполните вход в аккаунт", "Внимание",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }

                if (btnSync != null)
                {
                    btnSync.IsEnabled = false;
                    btnSync.Content = "🔄 синхронизация...";
                }

                var localTasks = GetAllTasks();

                System.Diagnostics.Debug.WriteLine("=== LOCAL TASKS BEFORE SYNC ===");
                foreach (var task in localTasks)
                {
                    System.Diagnostics.Debug.WriteLine($"  Task: {task.Title}, DueDate: {task.DueDate:yyyy-MM-dd HH:mm:ss}");
                }

                var (success, serverTasks) = await apiService.SyncTasks(localTasks);

                if (success && serverTasks != null)
                {
                    await Dispatcher.InvokeAsync(() =>
                    {
                        // Очищаем текущие коллекции
                        ActiveTasks.Clear();
                        CompletedTasks.Clear();

                        // Добавляем задачи с сервера
                        foreach (var serverTask in serverTasks)
                        {
                            System.Diagnostics.Debug.WriteLine($"Server task: {serverTask.Title}, DueDate: {serverTask.DueDate:yyyy-MM-dd HH:mm:ss}");

                            // Убеждаемся что дата не минимальная
                            if (serverTask.DueDate == DateTime.MinValue)
                            {
                                System.Diagnostics.Debug.WriteLine($"WARNING: Server task has MinValue date! Setting to default.");
                                serverTask.DueDate = DateTime.Now.AddDays(1).Date.AddHours(18);
                            }

                            serverTask.IsSynced = true;

                            if (serverTask.IsCompleted)
                                CompletedTasks.Add(serverTask);
                            else
                                ActiveTasks.Add(serverTask);
                        }

                        RefreshTaskLists();

                        if (SelectedTask != null && !ActiveTasks.Contains(SelectedTask) && !CompletedTasks.Contains(SelectedTask))
                        {
                            SelectedTask = null;
                            ShowNoTaskSelected();
                        }
                    });

                    settings.LastSync = DateTime.Now;
                    settings.Save();

                    if (showMessages)
                        MessageBox.Show("Синхронизация выполнена успешно", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                    return true;
                }
                else
                {
                    if (showMessages)
                        MessageBox.Show("Ошибка синхронизации. Проверьте подключение к серверу.", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (showMessages)
                    MessageBox.Show($"Ошибка синхронизации: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                isSyncing = false;
                if (btnSync != null)
                {
                    btnSync.IsEnabled = true;
                    btnSync.Content = "🔄 синхронизация";
                }
            }
        }

        public void RefreshTaskLists()
        {
            var sortedActive = ActiveTasks?.Where(t => t != null && t.DueDate != DateTime.MinValue)
                .OrderBy(t => t.DueDate)
                .ToList() ?? new List<TaskModel>();

            var sortedCompleted = CompletedTasks?.Where(t => t != null)
                .OrderByDescending(t => t.CompletionDate)
                .ToList() ?? new List<TaskModel>();

            ActiveTasks = new ObservableCollection<TaskModel>(sortedActive);
            CompletedTasks = new ObservableCollection<TaskModel>(sortedCompleted);
            UpdateTaskCounters();

            System.Diagnostics.Debug.WriteLine("=== AFTER REFRESH ===");
            foreach (var task in ActiveTasks)
            {
                System.Diagnostics.Debug.WriteLine($"  Active task: {task.Title}, DueDate: {task.DueDate:yyyy-MM-dd HH:mm:ss}");
            }
        }

        private void UpdateUIWithCurrentDate()
        {
            if (txtTodayDate != null)
            {
                var today = DateTime.Today;
                txtTodayDate.Text = today.ToString("dddd, d MMMM", new CultureInfo("ru-RU"));
            }

            if (txtTomorrow != null && txtDayAfter != null)
            {
                var today = DateTime.Today;
                txtTomorrow.Text = $"Завтра, {today.AddDays(1):d MMMM}";
                txtDayAfter.Text = $"Послезавтра, {today.AddDays(2):d MMMM}";
            }

            UpdateTaskCounters();
        }

        private void UpdateTaskCounters()
        {
            if (txtTomorrowCount == null || txtDayAfterCount == null) return;

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var dayAfterTomorrow = today.AddDays(2);

            var tomorrowCount = ActiveTasks?.Count(t => t != null && t.DueDate.Date == tomorrow) ?? 0;
            var tomorrowImportant = ActiveTasks?.Count(t => t != null && t.DueDate.Date == tomorrow && t.IsImportant) ?? 0;
            txtTomorrowCount.Text = $"{tomorrowCount} задач" +
                (tomorrowImportant > 0 ? $", {tomorrowImportant} важных" : "");

            var dayAfterCount = ActiveTasks?.Count(t => t != null && t.DueDate.Date == dayAfterTomorrow) ?? 0;
            var dayAfterImportant = ActiveTasks?.Count(t => t != null && t.DueDate.Date == dayAfterTomorrow && t.IsImportant) ?? 0;
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

            if (panelNoTaskSelected != null && panelTaskDetails != null)
            {
                panelNoTaskSelected.Visibility = Visibility.Collapsed;
                panelTaskDetails.Visibility = Visibility.Visible;
            }

            if (txtTaskTitle != null)
                txtTaskTitle.Text = SelectedTask.Title?.ToUpper() ?? "";

            if (txtTaskDescription != null)
                txtTaskDescription.Text = SelectedTask.Description ?? "";

            if (txtDueTime != null)
                txtDueTime.Text = $"Выполнить до {SelectedTask.DueTime}";

            if (txtCreatedTime != null)
                txtCreatedTime.Text = $"Создана в {SelectedTask.CreatedTime}";

            if (txtDateFull != null)
                txtDateFull.Text = SelectedTask.CreatedDate.ToString("d MMMM yyyy", new CultureInfo("ru-RU"));

            if (txtTimeFull != null)
                txtTimeFull.Text = SelectedTask.CreatedDate.ToString("HH:mm:ss");

            if (txtNotes != null)
                txtNotes.Text = SelectedTask.Notes ?? "";

            if (btnImportant != null)
                btnImportant.Content = SelectedTask.IsImportant ? "★ Важная" : "☆ Обычная";
        }

        private void ShowNoTaskSelected()
        {
            if (panelTaskDetails != null)
                panelTaskDetails.Visibility = Visibility.Collapsed;

            if (panelNoTaskSelected != null)
                panelNoTaskSelected.Visibility = Visibility.Visible;
        }

        public async void BtnSync_Click(object sender, RoutedEventArgs e)
        {
            await SyncWithServer(true);
        }

        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        {
            if (settings == null) settings = AppSettings.Load();

            if (string.IsNullOrEmpty(settings.Username))
            {
                var authWindow = new AuthWindow(settings);
                if (authWindow.ShowDialog() == true)
                {
                    settings.Username = authWindow.Username;
                    settings.Token = authWindow.Token;
                    settings.Save();

                    apiService.SetToken(settings.Token);

                    Dispatcher.BeginInvoke(new Action(async () => await SyncWithServer(true)));
                }
            }
            else
            {
                MessageBox.Show($"Вы вошли как: {settings.Username}\n" +
                    $"Последняя синхронизация: {(settings.LastSync == DateTime.MinValue ? "никогда" : settings.LastSync.ToString("dd.MM.yyyy HH:mm"))}",
                    "Аккаунт", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnTheme_Click(object sender, RoutedEventArgs e)
        {
            if (settings == null) settings = AppSettings.Load();
            settings.IsDarkTheme = !settings.IsDarkTheme;
            ApplyTheme(settings.IsDarkTheme);
            settings.Save();
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            if (settings == null) settings = AppSettings.Load();

            var settingsWindow = new SettingsWindow(settings, this);
            if (settingsWindow.ShowDialog() == true)
            {
                apiService = new ApiService(settings.ServerUrl);
                if (!string.IsNullOrEmpty(settings.Token))
                {
                    apiService.SetToken(settings.Token);
                }
            }
        }

        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new EditTaskWindow();
            if (editWindow.ShowDialog() == true)
            {
                var newTask = editWindow.EditedTask;

                if (string.IsNullOrEmpty(newTask.SyncId))
                {
                    newTask.SyncId = Guid.NewGuid().ToString();
                }
                newTask.IsSynced = false;

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
            if (task == null) return;

            var editWindow = new EditTaskWindow(task);
            if (editWindow.ShowDialog() == true)
            {
                var editedTask = editWindow.EditedTask;
                editedTask.SyncId = task.SyncId;
                editedTask.IsSynced = false;

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
                task.IsSynced = false;

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
                SelectedTask.IsSynced = false;
                UpdateTaskDetails();
            }
        }

        private void BtnListView_Click(object sender, RoutedEventArgs e)
        {
            if (btnListView != null && btnCardsView != null)
            {
                btnListView.Style = (Style)FindResource("SelectedButtonStyle");
                btnCardsView.Style = (Style)FindResource("ButtonStyle");
            }

            if (listTasks != null && cardsTasks != null)
            {
                listTasks.Visibility = Visibility.Visible;
                cardsTasks.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnCardsView_Click(object sender, RoutedEventArgs e)
        {
            if (btnCardsView != null && btnListView != null)
            {
                btnCardsView.Style = (Style)FindResource("SelectedButtonStyle");
                btnListView.Style = (Style)FindResource("ButtonStyle");
            }

            if (cardsTasks != null && listTasks != null)
            {
                cardsTasks.Visibility = Visibility.Visible;
                listTasks.Visibility = Visibility.Collapsed;
            }
        }

        private void TaskCard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is TaskModel task)
            {
                if (e.OriginalSource is Button || e.OriginalSource is System.Windows.Controls.TextBlock)
                    return;

                SelectedTask = task;
            }
        }

        private void TxtNotes_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (SelectedTask != null && txtNotes != null)
            {
                SelectedTask.Notes = txtNotes.Text;
                SelectedTask.IsSynced = false;
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool b && b) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}