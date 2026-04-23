using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Linq;

namespace TaskFlow
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private string _token;

        public ApiService(string baseUrl)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/")
            };
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public void SetToken(string token)
        {
            _token = token;
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
        }

        public async Task<(bool success, string message, UserData user, string token)> Login(string username, string password)
        {
            try
            {
                var loginData = new { username, password };
                var content = new StringContent(
                    JsonSerializer.Serialize(loginData),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync("api/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var result = JsonSerializer.Deserialize<LoginResponse>(responseContent, options);
                    if (result != null)
                    {
                        SetToken(result.access_token);
                        return (true, "Успешный вход", result.user, result.access_token);
                    }
                }
                else
                {
                    try
                    {
                        var error = JsonSerializer.Deserialize<ErrorResponse>(responseContent);
                        if (error != null && !string.IsNullOrEmpty(error.detail))
                            return (false, error.detail, null, null);
                    }
                    catch { }
                }
                return (false, "Ошибка входа", null, null);
            }
            catch (Exception ex)
            {
                return (false, $"Ошибка подключения: {ex.Message}", null, null);
            }
        }

        public async Task<(bool success, string newToken)> RefreshToken()
        {
            try
            {
                if (string.IsNullOrEmpty(_token))
                {
                    return (false, null);
                }

                var request = new HttpRequestMessage(HttpMethod.Post, "api/refresh-token");
                request.Headers.Add("Authorization", $"Bearer {_token}");

                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var result = JsonSerializer.Deserialize<RefreshTokenResponse>(responseContent, options);
                    if (result != null)
                    {
                        SetToken(result.access_token);
                        return (true, result.access_token);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Refresh token error: {ex.Message}");
            }
            return (false, null);
        }

        public async Task<(bool success, string message)> Register(string username, string email, string password, string recoveryWord)
        {
            try
            {
                var registerData = new { username, email, password, recovery_word = recoveryWord };
                var content = new StringContent(
                    JsonSerializer.Serialize(registerData),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync("api/register", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return (true, "Регистрация успешна");
                }
                else
                {
                    try
                    {
                        var error = JsonSerializer.Deserialize<ErrorResponse>(responseContent);
                        if (error != null && !string.IsNullOrEmpty(error.detail))
                            return (false, error.detail);
                    }
                    catch { }
                    return (false, "Ошибка регистрации");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Ошибка подключения: {ex.Message}");
            }
        }

        public async Task<(bool success, string message)> RecoverPassword(string username, string recoveryWord, string newPassword)
        {
            try
            {
                var recoverData = new { username, recovery_word = recoveryWord, new_password = newPassword };
                var content = new StringContent(
                    JsonSerializer.Serialize(recoverData),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync("api/recover-password", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return (true, "Пароль успешно изменен");
                }
                else
                {
                    try
                    {
                        var error = JsonSerializer.Deserialize<ErrorResponse>(responseContent);
                        if (error != null && !string.IsNullOrEmpty(error.detail))
                            return (false, error.detail);
                    }
                    catch { }
                    return (false, "Ошибка восстановления");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Ошибка подключения: {ex.Message}");
            }
        }

        public async Task<(bool success, List<TaskModel> tasks)> SyncTasks(List<TaskModel> localTasks)
        {
            try
            {
                if (string.IsNullOrEmpty(_token))
                {
                    return (false, null);
                }

                if (localTasks == null) localTasks = new List<TaskModel>();

                var validTasks = localTasks.Where(t => t != null).ToList();

                System.Diagnostics.Debug.WriteLine($"=== SENDING {validTasks.Count} TASKS ===");
                foreach (var task in validTasks)
                {
                    System.Diagnostics.Debug.WriteLine($"  Task: {task.Title}, SyncId: {task.SyncId}, DueDate: {task.DueDate:yyyy-MM-dd HH:mm:ss}");
                }

                var tasksForSync = new List<object>();
                foreach (var task in validTasks)
                {
                    var taskData = new
                    {
                        title = task.Title ?? "",
                        description = task.Description ?? "",
                        due_date = task.DueDate,
                        created_date = task.CreatedDate,
                        completion_date = task.IsCompleted ? task.CompletionDate : (DateTime?)null,
                        is_completed = task.IsCompleted,
                        is_important = task.IsImportant,
                        notes = task.Notes ?? "",
                        sync_id = task.SyncId  // ВАЖНО: отправляем существующий sync_id
                    };
                    tasksForSync.Add(taskData);
                }

                var syncData = new
                {
                    tasks = tasksForSync,
                    last_sync = DateTime.UtcNow
                };

                var options = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                string jsonData = JsonSerializer.Serialize(syncData, options);

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/sync", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"Sync response: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<ServerSyncResponse>(responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (result?.tasks != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"=== RECEIVED {result.tasks.Count} TASKS ===");
                        foreach (var task in result.tasks)
                        {
                            System.Diagnostics.Debug.WriteLine($"  Task: {task.Title}, SyncId: {task.SyncId}, DueDate: {task.DueDate:yyyy-MM-dd HH:mm:ss}");
                        }
                        return (true, result.tasks);
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var refreshResult = await RefreshToken();
                    if (refreshResult.success)
                    {
                        return await SyncTasks(localTasks);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Sync error: {ex.Message}");
            }
            return (false, null);
        }

        private class LoginResponse
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public UserData user { get; set; }
        }

        private class RefreshTokenResponse
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }

        private class ErrorResponse
        {
            public string detail { get; set; }
        }

        private class ServerSyncResponse
        {
            public List<TaskModel> tasks { get; set; }
            public DateTime sync_time { get; set; }
        }
    }

    public class UserData
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
    }
}