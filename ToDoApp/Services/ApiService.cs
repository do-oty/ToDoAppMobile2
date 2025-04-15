using System.Text.Json;
using System.Text;

namespace ToDoApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://todo-list.dcism.org";

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public class SignUpRequest
        {
            public required string first_name { get; set; }
            public required string last_name { get; set; }
            public required string email { get; set; }
            public required string password { get; set; }
            public required string confirm_password { get; set; }
        }

        public class SignInResponse
        {
            public int status { get; set; }
            public UserData? data { get; set; }
            public string? message { get; set; }
        }

        public class UserData
        {
            public int id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string password { get; set; }
            public string created_at { get; set; }
        }

        public class ApiResponse
        {
            public int status { get; set; }
            public string? message { get; set; }
        }

        public class TodoItem
        {
            public int item_id { get; set; }
            public required string item_name { get; set; }
            public required string item_description { get; set; }
            public required string status { get; set; }
            public int user_id { get; set; }
            public required string timemodified { get; set; }
        }

        public class TodoItemsResponse
        {
            public int status { get; set; }
            public Dictionary<string, TodoItem>? data { get; set; }
            public required string count { get; set; }
        }

        public async Task<ApiResponse> SignUpAsync(string firstName, string lastName, string email, string password, string confirmPassword)
        {
            try
            {
                var request = new SignUpRequest
                {
                    first_name = firstName,
                    last_name = lastName,
                    email = email,
                    password = password,
                    confirm_password = confirmPassword
                };
                
                var json = JsonSerializer.Serialize(request);
                System.Diagnostics.Debug.WriteLine($"=== SIGNUP REQUEST ===");
                System.Diagnostics.Debug.WriteLine($"URL: {BaseUrl}/signup_action.php");
                System.Diagnostics.Debug.WriteLine($"Data: {json}");
                
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                try
                {
                    var response = await _httpClient.PostAsync("/signup_action.php", content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"=== SIGNUP RESPONSE ===");
                    System.Diagnostics.Debug.WriteLine($"Status Code: {response.StatusCode}");
                    System.Diagnostics.Debug.WriteLine($"Response: {responseString}");
                    
                    return JsonSerializer.Deserialize<ApiResponse>(responseString);
                }
                catch (HttpRequestException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"=== NETWORK ERROR ===");
                    System.Diagnostics.Debug.WriteLine($"Error Type: {ex.GetType().Name}");
                    System.Diagnostics.Debug.WriteLine($"Status Code: {ex.StatusCode}");
                    System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"=== GENERAL ERROR ===");
                System.Diagnostics.Debug.WriteLine($"Error Type: {ex.GetType().Name}");
                System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<SignInResponse> SignInAsync(string email, string password)
        {
            try
            {
                var url = $"/signin_action.php?email={Uri.EscapeDataString(email)}&password={Uri.EscapeDataString(password)}";
                Console.WriteLine($"Sending sign-in request to: {BaseUrl}{url}");
                
                var response = await _httpClient.GetAsync(url);
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Sign-in response: {responseString}");
                
                return JsonSerializer.Deserialize<SignInResponse>(responseString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during sign-in: {ex.Message}");
                throw;
            }
        }

        public async Task<TodoItemsResponse> GetTodoItemsAsync(string status, int userId)
        {
            var response = await _httpClient.GetAsync($"/getItems_action.php?status={status}&user_id={userId}");
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TodoItemsResponse>(responseString);
        }

        public async Task<ApiResponse> AddTodoItemAsync(string itemName, string itemDescription, int userId)
        {
            var request = new
            {
                item_name = itemName,
                item_description = itemDescription,
                user_id = userId
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/addItem_action.php", content);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse>(responseString);
        }

        public async Task<ApiResponse> UpdateTodoItemAsync(string itemName, string itemDescription, int userId)
        {
            var request = new
            {
                item_name = itemName,
                item_description = itemDescription,
                user_id = userId
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("/editItem_action.php", content);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse>(responseString);
        }

        public async Task<ApiResponse> ChangeTodoItemStatusAsync(string status, int itemId)
        {
            var request = new
            {
                status = status,
                item_id = itemId
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("/statusItem_action.php", content);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse>(responseString);
        }

        public async Task<ApiResponse> DeleteTodoItemAsync(int itemId)
        {
            var response = await _httpClient.DeleteAsync($"/deleteItem_action.php?item_id={itemId}");
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse>(responseString);
        }
    }
} 