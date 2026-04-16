using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TaskFlow
{
    public partial class AuthWindow : Window
    {
        private ApiService _apiService;
        private AppSettings _settings;
        public bool IsAuthenticated { get; private set; }
        public string Username { get; private set; }
        public string Token { get; private set; }

        public AuthWindow(AppSettings settings)
        {
            InitializeComponent();
            _settings = settings;

            // Загружаем сохраненные настройки
            txtServerUrl.Text = settings.ServerUrl;
            chkRememberMe.IsChecked = settings.RememberMe;

            if (settings.RememberMe && !string.IsNullOrEmpty(settings.Username))
            {
                txtLoginUsername.Text = settings.Username;
                txtLoginPassword.Focus();
            }

            UpdateApiService();
        }

        private void UpdateApiService()
        {
            _apiService = new ApiService(txtServerUrl.Text);
        }

        private void TxtServerUrl_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdateApiService();
        }

        private void BtnLoginTab_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(btnLoginTab);
            panelLogin.Visibility = Visibility.Visible;
            panelRegister.Visibility = Visibility.Collapsed;
            panelRecover.Visibility = Visibility.Collapsed;
            txtStatus.Text = "";
        }

        private void BtnRegisterTab_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(btnRegisterTab);
            panelLogin.Visibility = Visibility.Collapsed;
            panelRegister.Visibility = Visibility.Visible;
            panelRecover.Visibility = Visibility.Collapsed;
            txtStatus.Text = "";
        }

        private void BtnRecoverTab_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(btnRecoverTab);
            panelLogin.Visibility = Visibility.Collapsed;
            panelRegister.Visibility = Visibility.Collapsed;
            panelRecover.Visibility = Visibility.Visible;
            txtStatus.Text = "";
        }

        private void SetActiveTab(Button activeButton)
        {
            btnLoginTab.Style = (Style)FindResource("TabButtonStyle");
            btnRegisterTab.Style = (Style)FindResource("TabButtonStyle");
            btnRecoverTab.Style = (Style)FindResource("TabButtonStyle");
            activeButton.Style = (Style)FindResource("ActiveTabButtonStyle");
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLoginUsername.Text) ||
                string.IsNullOrWhiteSpace(txtLoginPassword.Password))
            {
                txtStatus.Text = "Заполните все поля";
                txtStatus.Foreground = new SolidColorBrush(Color.FromRgb(255, 68, 68));
                return;
            }

            try
            {
                btnLogin.IsEnabled = false;
                txtStatus.Text = "Подключение к серверу...";
                txtStatus.Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102));

                var (success, message, user, accessToken) = await _apiService.Login(
                    txtLoginUsername.Text,
                    txtLoginPassword.Password
                );

                if (success)
                {
                    // Сохраняем настройки
                    _settings.ServerUrl = txtServerUrl.Text;
                    _settings.RememberMe = chkRememberMe.IsChecked == true;
                    _settings.Username = _settings.RememberMe ? user.username : "";
                    _settings.Token = accessToken; // Сохраняем настоящий JWT токен!
                    _settings.Save();

                    Username = user.username;
                    Token = accessToken; // Передаем настоящий токен
                    IsAuthenticated = true;

                    DialogResult = true;
                    Close();
                }
                else
                {
                    txtStatus.Text = message;
                    txtStatus.Foreground = new SolidColorBrush(Color.FromRgb(255, 68, 68));
                }
            }
            catch (Exception ex)
            {
                txtStatus.Text = $"Ошибка: {ex.Message}";
                txtStatus.Foreground = new SolidColorBrush(Color.FromRgb(255, 68, 68));
            }
            finally
            {
                btnLogin.IsEnabled = true;
            }
        }

        private async void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRegUsername.Text) ||
                string.IsNullOrWhiteSpace(txtRegEmail.Text) ||
                string.IsNullOrWhiteSpace(txtRegPassword.Password) ||
                string.IsNullOrWhiteSpace(txtRegConfirmPassword.Password) ||
                string.IsNullOrWhiteSpace(txtRecoveryWord.Text))
            {
                txtStatus.Text = "Заполните все поля";
                txtStatus.Foreground = new SolidColorBrush(Color.FromRgb(255, 68, 68));
                return;
            }

            if (txtRegPassword.Password != txtRegConfirmPassword.Password)
            {
                txtStatus.Text = "Пароли не совпадают";
                txtStatus.Foreground = new SolidColorBrush(Color.FromRgb(255, 68, 68));
                return;
            }

            try
            {
                btnRegister.IsEnabled = false;
                txtStatus.Text = "Регистрация...";
                txtStatus.Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102));

                var (success, message) = await _apiService.Register(
                    txtRegUsername.Text,
                    txtRegEmail.Text,
                    txtRegPassword.Password,
                    txtRecoveryWord.Text
                );

                txtStatus.Text = message;
                txtStatus.Foreground = success ?
                    new SolidColorBrush(Color.FromRgb(76, 175, 80)) :
                    new SolidColorBrush(Color.FromRgb(255, 68, 68));

                if (success)
                {
                    // Переключаемся на вкладку входа
                    BtnLoginTab_Click(null, null);
                    txtLoginUsername.Text = txtRegUsername.Text;
                }
            }
            catch (Exception ex)
            {
                txtStatus.Text = $"Ошибка: {ex.Message}";
                txtStatus.Foreground = new SolidColorBrush(Color.FromRgb(255, 68, 68));
            }
            finally
            {
                btnRegister.IsEnabled = true;
            }
        }

        private async void BtnRecover_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRecoverUsername.Text) ||
                string.IsNullOrWhiteSpace(txtRecoverWord.Text) ||
                string.IsNullOrWhiteSpace(txtRecoverNewPassword.Password) ||
                string.IsNullOrWhiteSpace(txtRecoverConfirmPassword.Password))
            {
                txtStatus.Text = "Заполните все поля";
                txtStatus.Foreground = new SolidColorBrush(Color.FromRgb(255, 68, 68));
                return;
            }

            if (txtRecoverNewPassword.Password != txtRecoverConfirmPassword.Password)
            {
                txtStatus.Text = "Пароли не совпадают";
                txtStatus.Foreground = new SolidColorBrush(Color.FromRgb(255, 68, 68));
                return;
            }

            try
            {
                btnRecover.IsEnabled = false;
                txtStatus.Text = "Восстановление пароля...";
                txtStatus.Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102));

                var (success, message) = await _apiService.RecoverPassword(
                    txtRecoverUsername.Text,
                    txtRecoverWord.Text,
                    txtRecoverNewPassword.Password
                );

                txtStatus.Text = message;
                txtStatus.Foreground = success ?
                    new SolidColorBrush(Color.FromRgb(76, 175, 80)) :
                    new SolidColorBrush(Color.FromRgb(255, 68, 68));

                if (success)
                {
                    // Переключаемся на вкладку входа
                    BtnLoginTab_Click(null, null);
                    txtLoginUsername.Text = txtRecoverUsername.Text;
                }
            }
            catch (Exception ex)
            {
                txtStatus.Text = $"Ошибка: {ex.Message}";
                txtStatus.Foreground = new SolidColorBrush(Color.FromRgb(255, 68, 68));
            }
            finally
            {
                btnRecover.IsEnabled = true;
            }
        }
    }
}