using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://todo-list.dcism.org";

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
            Debug.WriteLine($"[ApiService] Initialized with base URL: {BaseUrl}");
        }

        public async Task<ApiResponse<object>> SignUpAsync(string firstName, string lastName, string email, string password, string confirmPassword)
        {
            var request = new SignUpRequest
            {
                first_name = firstName,
                last_name = lastName,
                email = email,
                password = password,
                confirm_password = confirmPassword
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/signup_action.php", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<object>>(responseContent);
        }

        public async Task<ApiResponse<UserData>> SignInAsync(string email, string password)
        {
            var response = await _httpClient.GetAsync($"/signin_action.php?email={Uri.EscapeDataString(email)}&password={Uri.EscapeDataString(password)}");
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<UserData>>(responseContent);
        }

        public async Task<ApiResponse<Dictionary<string, TaskItemData>>> GetTodoItemsAsync(string status, int userId)
        {
            var response = await _httpClient.GetAsync($"/getItems_action.php?status={status}&user_id={userId}");
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<Dictionary<string, TaskItemData>>>(responseContent);
        }

        public async Task<ApiResponse<TaskItemData>> AddTodoItemAsync(AddTaskRequest request)
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/addItem_action.php", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<TaskItemData>>(responseContent);
        }

        public async Task<ApiResponse<object>> UpdateTodoItemAsync(int itemId, string itemName, string itemDescription)
        {
            var request = new UpdateTaskRequest
            {
                item_id = itemId,
                item_name = itemName,
                item_description = itemDescription
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("/editItem_action.php", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<object>>(responseContent);
        }

        public async Task<ApiResponse<object>> UpdateTaskStatusAsync(int itemId, string status)
        {
            var request = new { status, item_id = itemId };
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("/statusItem_action.php", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<object>>(responseContent);
        }

        public async Task<ApiResponse<object>> DeleteTodoItemAsync(int itemId)
        {
            var response = await _httpClient.DeleteAsync($"/deleteItem_action.php?item_id={itemId}");
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<object>>(responseContent);
        }

        public async Task<ApiResponse<UserData>> GetUserDataAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/users/{userId}");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<UserData>>(content);
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserData>
                {
                    status = 500,
                    message = $"Error: {ex.Message}"
                };
            }
        }
    }

    public class ApiResponse<T>
    {
        public int status { get; set; }
        public T data { get; set; }
        public string message { get; set; }
    }

    public class SignUpRequest
    {
        [JsonPropertyName("first_name")]
        public string first_name { get; set; }

        [JsonPropertyName("last_name")]
        public string last_name { get; set; }

        [JsonPropertyName("email")]
        public string email { get; set; }

        [JsonPropertyName("password")]
        public string password { get; set; }

        [JsonPropertyName("confirm_password")]
        public string confirm_password { get; set; }
    }

    public class TaskItemData
    {
        public int? item_id { get; set; }
        public string item_name { get; set; }
        public string item_description { get; set; }
        public string status { get; set; }
        public int? user_id { get; set; }
        public string timemodified { get; set; }
    }

    public class AddTaskRequest
    {
        public string item_name { get; set; }
        public string item_description { get; set; }
        public int user_id { get; set; }
    }

    public class UpdateTaskRequest
    {
        public string item_name { get; set; }
        public string item_description { get; set; }
        public int item_id { get; set; }
    }
} 