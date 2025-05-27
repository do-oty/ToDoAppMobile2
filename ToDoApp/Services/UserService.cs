using System.Net.Http.Json;
using System.Text.Json;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://todo-list.dcism.org";

        public UserService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<(bool success, string message)> SignUpAsync(string firstName, string lastName, string email, string password, string confirmPassword)
        {
            try
            {
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
                var result = JsonSerializer.Deserialize<ApiResponse>(content);

                return (result?.Status == 200, result?.Message ?? "Unknown error occurred");
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<(bool success, UserData? userData, string message)> SignInAsync(string email, string password)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/signin_action.php?email={Uri.EscapeDataString(email)}&password={Uri.EscapeDataString(password)}");
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<SignInResponse>(content);

                if (result?.Status == 200 && result.Data != null)
                {
                    return (true, result.Data, result.Message);
                }

                return (false, null, result?.Message ?? "Unknown error occurred");
            }
            catch (Exception ex)
            {
                return (false, null, $"Error: {ex.Message}");
            }
        }
    }
} 