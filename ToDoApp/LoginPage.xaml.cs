using Microsoft.Maui.Controls;
using ToDoApp.Services;

namespace ToDoApp;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _apiService;
    private bool _isPasswordVisible = false;

    public LoginPage()
    {
        InitializeComponent();
        _apiService = new ApiService();
        PasswordToggleButton.Clicked += OnPasswordToggleClicked;
    }

    private void OnPasswordToggleClicked(object? sender, EventArgs e)
    {
        _isPasswordVisible = !_isPasswordVisible;
        PasswordEntry.IsPassword = !_isPasswordVisible;
        PasswordToggleButton.Source = _isPasswordVisible ? "eye.svg" : "eyebrow.svg";
    }

    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EmailEntry.Text))
        {
            await DisplayAlert("Error", "Please enter your email", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert("Error", "Please enter your password", "OK");
            return;
        }

        try
        {
            var email = EmailEntry.Text.Trim();
            var password = PasswordEntry.Text;
            
            Console.WriteLine($"Attempting login with email: {email}"); // Debug logging
            var response = await _apiService.SignInAsync(email, password);
            
            Console.WriteLine($"Login response - Status: {response.status}, Message: {response.message}"); // Debug logging

            if (response.status == 200 && response.data != null)
            {
                // Store user data in preferences
                Preferences.Set("UserId", response.data.id.ToString());
                Preferences.Set("UserName", $"{response.data.first_name} {response.data.last_name}");
                Preferences.Set("UserEmail", response.data.email);

                Console.WriteLine($"Login successful - User: {response.data.first_name} {response.data.last_name}"); // Debug logging

                // Navigate to dashboard
                await Shell.Current.GoToAsync("//DashboardPage");
            }
            else
            {
                var errorMessage = response.message ?? "Account does not exist or invalid credentials";
                Console.WriteLine($"Login failed - Error: {errorMessage}"); // Debug logging
                await DisplayAlert("Error", errorMessage, "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}"); // Debug logging
            await DisplayAlert("Error", "An error occurred while signing in. Please try again.", "OK");
        }
    }

    private async void OnSignUpTapped(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage());
    }

    private async void OnGoogleLoginTapped(object? sender, EventArgs e)
    {
        // For now, just navigate directly to the dashboard
        await Shell.Current.GoToAsync("//DashboardPage");
    }
}