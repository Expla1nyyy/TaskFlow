using System;
using System.IO;
using System.Text.Json;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace TaskFlow
{
    public class AppSettings : INotifyPropertyChanged
    {
        private static readonly string SettingsFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "TaskFlow", "settings.json");

        private bool _isDarkTheme;
        private string _username;
        private string _token;
        private string _serverUrl = "http://localhost:8000";
        private bool _rememberMe;
        private DateTime _lastSync;

        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set { _isDarkTheme = value; OnPropertyChanged(); }
        }

        public string Username
        {
            get => _username ?? "";
            set { _username = value; OnPropertyChanged(); }
        }

        public string Token
        {
            get => _token ?? "";
            set { _token = value; OnPropertyChanged(); }
        }

        public string ServerUrl
        {
            get => _serverUrl ?? "http://localhost:8000";
            set { _serverUrl = value; OnPropertyChanged(); }
        }

        public bool RememberMe
        {
            get => _rememberMe;
            set { _rememberMe = value; OnPropertyChanged(); }
        }

        public DateTime LastSync
        {
            get => _lastSync;
            set { _lastSync = value; OnPropertyChanged(); }
        }

        public static AppSettings Load()
        {
            try
            {
                if (File.Exists(SettingsFile))
                {
                    string json = File.ReadAllText(SettingsFile);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        return JsonSerializer.Deserialize<AppSettings>(json, options) ?? new AppSettings();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings: {ex.Message}");
            }
            return new AppSettings();
        }

        public void Save()
        {
            try
            {
                string directory = Path.GetDirectoryName(SettingsFile);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                string json = JsonSerializer.Serialize(this, options);
                File.WriteAllText(SettingsFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}