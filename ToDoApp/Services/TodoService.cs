using System.Net.Http.Json;
using System.Text.Json;
using System.Diagnostics;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class TodoService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://todo-list.dcism.org";

        public TodoService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
            Debug.WriteLine($"[TodoService] Initialized with base URL: {BaseUrl}");
        }

        public async Task<(bool success, string message)> SignUpAsync(string firstName, string lastName, string email, string password, string confirmPassword)
        {
            try
            {
                Debug.WriteLine($"[TodoService] Signing up user - Email: {email}");
                var signUpData = new
                {
                    first_name = firstName,
                    last_name = lastName,
                    email = email,
                    password = password,
                    confirm_password = confirmPassword
                };

                var response = await _httpClient.PostAsJsonAsync("/signup_action.php", signUpData);
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[TodoService] Raw API Response: {content}");
                
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                var result = JsonSerializer.Deserialize<ApiResponse>(content, options);
                Debug.WriteLine($"[TodoService] Deserialized Response - Status: {result?.Status}, Message: {result?.Message}");

                if (result?.Status == 200)
                {
                    Debug.WriteLine("[TodoService] Sign up successful");
                    return (true, result.Message ?? "Account created successfully");
                }

                Debug.WriteLine($"[TodoService] Sign up failed. Status: {result?.Status}, Message: {result?.Message}");
                return (false, result?.Message ?? "Unknown error occurred");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TodoService] Error signing up: {ex}");
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<(bool success, UserData? userData, string message)> SignInAsync(string email, string password)
        {
            try
            {
                Debug.WriteLine($"[TodoService] Signing in user - Email: {email}");
                var url = $"/signin_action.php?email={Uri.EscapeDataString(email)}&password={Uri.EscapeDataString(password)}";
                Debug.WriteLine($"[TodoService] Request URL: {url}");
                
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[TodoService] Raw API Response: {content}");
                
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                var result = JsonSerializer.Deserialize<SignInResponse>(content, options);
                Debug.WriteLine($"[TodoService] Deserialized Response - Status: {result?.Status}, Message: {result?.Message}");

                if (result?.Status == 200 && result.Data != null)
                {
                    Debug.WriteLine($"[TodoService] Sign in successful for user ID: {result.Data.Id}");
                    return (true, result.Data, result.Message ?? "Success");
                }

                Debug.WriteLine($"[TodoService] Sign in failed. Status: {result?.Status}, Message: {result?.Message}");
                return (false, null, result?.Message ?? "Unknown error occurred");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TodoService] Error signing in: {ex}");
                return (false, null, $"Error: {ex.Message}");
            }
        }

        public async Task<(bool success, List<TodoItem> items, string message)> GetTodoItemsAsync(string status, int userId)
        {
            try
            {
                Debug.WriteLine($"[TodoService] Getting todo items for status: {status}, userId: {userId}");
                var url = $"/getItems_action.php?status={status}&user_id={userId}";
                Debug.WriteLine($"[TodoService] Request URL: {url}");
                
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[TodoService] Raw API Response: {content}");
                
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                var result = JsonSerializer.Deserialize<TodoItemsResponse>(content, options);
                Debug.WriteLine($"[TodoService] Deserialized Response - Status: {result?.Status}, Message: {result?.Message}, Data Count: {result?.Data?.Count}");

                if (result?.Status == 200 && result.Data != null)
                {
                    var items = result.Data.Values.ToList();
                    Debug.WriteLine($"[TodoService] Successfully retrieved {items.Count} items");
                    return (true, items, result.Message ?? "Success");
                }

                Debug.WriteLine($"[TodoService] Failed to get items. Status: {result?.Status}, Message: {result?.Message}");
                return (false, new List<TodoItem>(), result?.Message ?? "Unknown error occurred");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TodoService] Error getting todo items: {ex}");
                return (false, new List<TodoItem>(), $"Error: {ex.Message}");
            }
        }

        public async Task<(bool success, TodoItem? item, string message)> AddTodoItemAsync(string itemName, string itemDescription, int userId)
        {
            try
            {
                Debug.WriteLine($"[TodoService] Adding todo item - Name: {itemName}, Description: {itemDescription}, UserId: {userId}");
                var todoData = new
                {
                    item_name = itemName,
                    item_description = itemDescription,
                    user_id = userId
                };

                var response = await _httpClient.PostAsJsonAsync("/addItem_action.php", todoData);
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[TodoService] Raw API Response: {content}");
                
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                var result = JsonSerializer.Deserialize<AddTodoResponse>(content, options);
                Debug.WriteLine($"[TodoService] Deserialized Response - Status: {result?.Status}, Message: {result?.Message}");

                if (result?.Status == 200 && result.Data != null)
                {
                    Debug.WriteLine($"[TodoService] Successfully added item with ID: {result.Data.ItemId}");
                    return (true, result.Data, result.Message ?? "Success");
                }

                Debug.WriteLine($"[TodoService] Failed to add item. Status: {result?.Status}, Message: {result?.Message}");
                return (false, null, result?.Message ?? "Unknown error occurred");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TodoService] Error adding todo item: {ex}");
                return (false, null, $"Error: {ex.Message}");
            }
        }

        public async Task<(bool success, string message)> UpdateTodoItemAsync(string itemName, string itemDescription, int itemId)
        {
            try
            {
                Debug.WriteLine($"[TodoService] Updating todo item - ID: {itemId}, Name: {itemName}, Description: {itemDescription}");
                var todoData = new
                {
                    item_name = itemName,
                    item_description = itemDescription,
                    item_id = itemId
                };

                var response = await _httpClient.PutAsJsonAsync("/editItem_action.php", todoData);
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[TodoService] Raw API Response: {content}");
                
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                var result = JsonSerializer.Deserialize<ApiResponse>(content, options);
                Debug.WriteLine($"[TodoService] Deserialized Response - Status: {result?.Status}, Message: {result?.Message}");

                if (result?.Status == 200)
                {
                    Debug.WriteLine($"[TodoService] Successfully updated item {itemId}");
                    return (true, result.Message ?? "Success");
                }

                Debug.WriteLine($"[TodoService] Failed to update item. Status: {result?.Status}, Message: {result?.Message}");
                return (false, result?.Message ?? "Unknown error occurred");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TodoService] Error updating todo item: {ex}");
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<(bool success, string message)> ChangeTodoStatusAsync(string status, int itemId)
        {
            try
            {
                Debug.WriteLine($"[TodoService] Changing todo status - ID: {itemId}, Status: {status}");
                var statusData = new
                {
                    status = status,
                    item_id = itemId
                };

                var response = await _httpClient.PutAsJsonAsync("/statusItem_action.php", statusData);
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[TodoService] Raw API Response: {content}");
                
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                var result = JsonSerializer.Deserialize<ApiResponse>(content, options);
                Debug.WriteLine($"[TodoService] Deserialized Response - Status: {result?.Status}, Message: {result?.Message}");

                if (result?.Status == 200)
                {
                    Debug.WriteLine($"[TodoService] Successfully changed status for item {itemId}");
                    return (true, result.Message ?? "Success");
                }

                Debug.WriteLine($"[TodoService] Failed to change status. Status: {result?.Status}, Message: {result?.Message}");
                return (false, result?.Message ?? "Unknown error occurred");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TodoService] Error changing todo status: {ex}");
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<(bool success, string message)> DeleteTodoItemAsync(int itemId)
        {
            try
            {
                Debug.WriteLine($"[TodoService] Deleting todo item - ID: {itemId}");
                var url = $"/deleteItem_action.php?item_id={itemId}";
                Debug.WriteLine($"[TodoService] Request URL: {url}");
                
                var response = await _httpClient.DeleteAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[TodoService] Raw API Response: {content}");
                
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                var result = JsonSerializer.Deserialize<ApiResponse>(content, options);
                Debug.WriteLine($"[TodoService] Deserialized Response - Status: {result?.Status}, Message: {result?.Message}");

                if (result?.Status == 200)
                {
                    Debug.WriteLine($"[TodoService] Successfully deleted item {itemId}");
                    return (true, result.Message ?? "Success");
                }

                Debug.WriteLine($"[TodoService] Failed to delete item. Status: {result?.Status}, Message: {result?.Message}");
                return (false, result?.Message ?? "Unknown error occurred");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TodoService] Error deleting todo item: {ex}");
                return (false, $"Error: {ex.Message}");
            }
        }
    }
} 