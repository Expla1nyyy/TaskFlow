using System.Windows;
using System.Windows.Media;

namespace TaskFlow
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Применяем тему при запуске приложения
            var settings = AppSettings.Load();
            if (settings != null && settings.IsDarkTheme)
            {
                ApplyTheme(true);
            }
        }

        public static void ApplyTheme(bool isDark)
        {
            var resources = Application.Current.Resources;

            if (isDark)
            {
                resources["BackgroundBrush"] = new SolidColorBrush(Color.FromRgb(30, 30, 30));
                resources["CardBackground"] = new SolidColorBrush(Color.FromRgb(45, 45, 45));
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
}