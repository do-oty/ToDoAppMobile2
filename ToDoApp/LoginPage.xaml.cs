using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.Text.Json;
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
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("No Internet", "Please check your internet connection and try again.", "OK");
            return;
        }

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
            // Show loading state
            LoadingOverlay.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            LoginButton.IsEnabled = false;

            var email = EmailEntry.Text.Trim();
            var password = PasswordEntry.Text;

            Debug.WriteLine($"Attempting login with email: {email}");
            var response = await _apiService.SignInAsync(email, password);

            Debug.WriteLine($"Login response - Status: {response.status}, Message: {response.message}");

            if (response.status == 200 && response.data != null)
            {
                // Store user data securely
                await SecureStorage.SetAsync("auth_token", "your_auth_token_if_available");
                Preferences.Set("UserId", response.data.id.ToString());
                Preferences.Set("UserName", $"{response.data.first_name} {response.data.last_name}");
                Preferences.Set("UserEmail", response.data.email);
                Preferences.Set("FirstName", response.data.first_name);
                Preferences.Set("LastName", response.data.last_name);


                Debug.WriteLine($"Login successful - User ID: {response.data.id}");

                // Navigate to dashboard with user ID
                await Shell.Current.GoToAsync($"//DashboardPage?userId={response.data.id}");
            }
            else
            {
                var errorMessage = response.message ?? "Invalid email or password";
                Debug.WriteLine($"Login failed - Error: {errorMessage}");
                await DisplayAlert("Login Failed", errorMessage, "OK");
            }
        }
        catch (HttpRequestException httpEx)
        {
            Debug.WriteLine($"Network error: {httpEx}");
            await DisplayAlert("Network Error", "Couldn't connect to the server. Please try again later.", "OK");
        }
        catch (JsonException jsonEx)
        {
            Debug.WriteLine($"JSON error: {jsonEx}");
            await DisplayAlert("Error", "Invalid server response. Please try again.", "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unexpected error: {ex}");
            await DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
        }
        finally
        {
            // Reset loading state
            LoadingOverlay.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            LoginButton.IsEnabled = true;
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Clear the input fields on every appearance of the LoginPage
        EmailEntry.Text = string.Empty;
        PasswordEntry.Text = string.Empty;
    }
    private async void OnSignUpTapped(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage());
    }
}