using System;
using System.Windows;
using System.Windows.Media;

namespace TaskFlow
{
    public partial class SettingsWindow : Window
    {
        private AppSettings _settings;
        private MainWindow _mainWindow;

        public SettingsWindow(AppSettings settings, MainWindow mainWindow)
        {
            try
            {
                InitializeComponent();

                // Проверяем входные параметры
                _settings = settings ?? AppSettings.Load();
                _mainWindow = mainWindow;

                // Загружаем настройки
                LoadSettings();

                // Подписываемся на события
                this.Loaded += SettingsWindow_Loaded;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации окна настроек: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                this.DialogResult = false;
                this.Close();
            }
        }

        private void SettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Применяем тему после загрузки окна
                ApplyCurrentTheme();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при загрузке окна: {ex.Message}");
            }
        }

        private void LoadSettings()
        {
            try
            {
                if (_settings != null)
                {
                    chkDarkTheme.IsChecked = _settings.IsDarkTheme;
                    txtServerUrl.Text = _settings.ServerUrl ?? "http://localhost:8000";
                    txtCurrentUser.Text = string.IsNullOrEmpty(_settings.Username) ?
                        "Не выполнен вход" : _settings.Username;

                    UpdateLastSyncText();
                }
                else
                {
                    chkDarkTheme.IsChecked = false;
                    txtServerUrl.Text = "http://localhost:8000";
                    txtCurrentUser.Text = "Не выполнен вход";
                    txtLastSync.Text = "Последняя синхронизация: никогда";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки настроек: {ex.Message}");
            }
        }

        private void ApplyCurrentTheme()
        {
            try
            {
                if (_settings != null)
                {
                    var resources = Application.Current.Resources;

                    if (_settings.IsDarkTheme)
                    {
                        resources["BackgroundBrush"] = new SolidColorBrush(Color.FromRgb(45, 45, 45));
                        resources["CardBackground"] = new SolidColorBrush(Color.FromRgb(30, 30, 30));
                        resources["TextColor"] = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        resources["SecondaryText"] = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                        resources["BorderColor"] = new SolidColorBrush(Color.FromRgb(70, 70, 70));
                    }
                    else
                    {
                        resources["BackgroundBrush"] = new SolidColorBrush(Color.FromRgb(245, 245, 245));
                        resources["CardBackground"] = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        resources["TextColor"] = new SolidColorBrush(Color.FromRgb(51, 51, 51));
                        resources["SecondaryText"] = new SolidColorBrush(Color.FromRgb(102, 102, 102));
                        resources["BorderColor"] = new SolidColorBrush(Color.FromRgb(224, 224, 224));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка применения темы: {ex.Message}");
            }
        }

        private void UpdateLastSyncText()
        {
            try
            {
                if (_settings != null)
                {
                    if (_settings.LastSync == DateTime.MinValue)
                        txtLastSync.Text = "Последняя синхронизация: никогда";
                    else
                        txtLastSync.Text = $"Последняя синхронизация: {_settings.LastSync:dd.MM.yyyy HH:mm}";
                }
                else
                {
                    txtLastSync.Text = "Последняя синхронизация: никогда";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка обновления текста синхронизации: {ex.Message}");
                txtLastSync.Text = "Последняя синхронизация: неизвестно";
            }
        }

        private void ChkDarkTheme_Changed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_settings != null)
                {
                    _settings.IsDarkTheme = chkDarkTheme.IsChecked == true;
                    ApplyCurrentTheme();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при изменении темы: {ex.Message}");
            }
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_settings?.Username))
                {
                    MessageBox.Show("Сначала выполните вход в аккаунт", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MessageBox.Show("Функция смены пароля будет доступна в следующей версии",
                    "Смена пароля", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnSyncNow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_settings == null || string.IsNullOrEmpty(_settings.Token))
                {
                    MessageBox.Show("Сначала выполните вход в аккаунт", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                btnSyncNow.IsEnabled = false;
                txtLastSync.Text = "Синхронизация...";

                var apiService = new ApiService(_settings.ServerUrl ?? "http://localhost:8000");
                apiService.SetToken(_settings.Token);

                var tasksForSync = _mainWindow?.GetAllTasks() ?? new System.Collections.Generic.List<TaskModel>();

                // Явно указываем типы при деконструкции
                var result = await apiService.SyncTasks(tasksForSync);
                bool success = result.success;
                var tasks = result.tasks;

                if (success && tasks != null)
                {
                    // В MainWindow синхронизация уже выполнена через apiService.SyncTasks
                    // Просто обновляем время последней синхронизации
                    _settings.LastSync = DateTime.Now;
                    _settings.Save();

                    // Предлагаем пользователю перезапустить синхронизацию из главного окна для обновления UI
                    MessageBox.Show("Синхронизация выполнена успешно.\nНажмите кнопку синхронизации в главном окне для обновления задач.",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Ошибка синхронизации. Проверьте подключение к серверу.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка синхронизации: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                UpdateLastSyncText();
                btnSyncNow.IsEnabled = true;
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_settings == null) return;

                var result = MessageBox.Show("Вы уверены, что хотите выйти из аккаунта?\nВсе несинхронизированные задачи будут сохранены локально.",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _settings.Token = "";
                    _settings.Username = "";
                    _settings.RememberMe = false;
                    _settings.Save();

                    // Очищаем задачи в главном окне
                    if (_mainWindow != null)
                    {
                        _mainWindow.ActiveTasks?.Clear();
                        _mainWindow.CompletedTasks?.Clear();
                    }

                    MessageBox.Show("Вы вышли из аккаунта", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выходе: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_settings != null)
                {
                    _settings.ServerUrl = txtServerUrl.Text?.Trim() ?? "http://localhost:8000";
                    _settings.Save();
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = false;
                this.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при закрытии: {ex.Message}");
                this.Close();
            }
        }
    }
}