using Microsoft.Maui.Controls;
using Microsoft.Maui.Networking;
using System.Diagnostics;
using ToDoApp.Services;
using System.ComponentModel;

namespace ToDoApp;

public partial class LoginPage : ContentPage, INotifyPropertyChanged
{
    private readonly UserService _userService;
    private bool _isPasswordVisible = false;
    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    public LoginPage()
    {
        InitializeComponent();
        _userService = Application.Current.Handler.MauiContext.Services.GetService<UserService>();
        BindingContext = this;
    }

    private void OnPasswordToggleClicked(object sender, EventArgs e)
    {
        _isPasswordVisible = !_isPasswordVisible;
        PasswordEntry.IsPassword = !_isPasswordVisible;
        ((Button)sender).Text = _isPasswordVisible ? "🙈" : "👁️";
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("No Internet", "Please check your internet connection and try again.", "OK");
            return;
        }

        var email = EmailEntry.Text;
        var password = PasswordEntry.Text;
        if (string.IsNullOrWhiteSpace(email))
        {
            await DisplayAlert("Error", "Please enter your email", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Please enter your password", "OK");
            return;
        }
        try
        {
            IsLoading = true;
            LoginButton.IsEnabled = false;
            var result = await _userService.SignInAsync(email, password);
            bool success = result.success;
            var userData = result.userData;
            string message = result.message;
            if (success && userData != null)
            {
                Preferences.Set("UserId", userData.Id.ToString());
                Preferences.Set("UserName", $"{userData.FirstName} {userData.LastName}");
                Preferences.Set("UserEmail", userData.Email);
                Preferences.Set("FirstName", userData.FirstName);
                Preferences.Set("LastName", userData.LastName);
                await Shell.Current.GoToAsync($"///DashboardPage?userId={userData.Id}");
            }
            else
            {
                await DisplayAlert("Login Failed", message, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
        }
        finally
        {
            IsLoading = false;
            LoginButton.IsEnabled = true;
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        EmailEntry.Text = string.Empty;
        PasswordEntry.Text = string.Empty;
        Preferences.Clear();
    }

    private async void OnSignUpTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///RegisterPage");
    }
}