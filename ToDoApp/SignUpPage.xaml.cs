using ToDoApp.Services;

namespace ToDoApp;

public partial class SignUpPage : ContentPage
{
    private readonly ApiService _apiService;
    private bool _isPasswordVisible = false;
    private bool _isConfirmPasswordVisible = false;

    public SignUpPage()
    {
        InitializeComponent();
        _apiService = new ApiService();
        PasswordToggleButton.Clicked += OnPasswordToggleClicked;
        ConfirmPasswordToggleButton.Clicked += OnConfirmPasswordToggleClicked;
    }

    private void OnPasswordToggleClicked(object? sender, EventArgs e)
    {
        _isPasswordVisible = !_isPasswordVisible;
        PasswordEntry.IsPassword = !_isPasswordVisible;
        PasswordToggleButton.Source = _isPasswordVisible ? "eye.svg" : "eyebrow.svg";
    }

    private void OnConfirmPasswordToggleClicked(object? sender, EventArgs e)
    {
        _isConfirmPasswordVisible = !_isConfirmPasswordVisible;
        ConfirmPasswordEntry.IsPassword = !_isConfirmPasswordVisible;
        ConfirmPasswordToggleButton.Source = _isConfirmPasswordVisible ? "eye.svg" : "eyebrow.svg";
    }

    private async void OnSignUpClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FirstNameEntry.Text))
        {
            await DisplayAlert("Error", "Please enter your first name", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(LastNameEntry.Text))
        {
            await DisplayAlert("Error", "Please enter your last name", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(EmailEntry.Text))
        {
            await DisplayAlert("Error", "Please enter an email", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert("Error", "Please enter a password", "OK");
            return;
        }

        if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            await DisplayAlert("Error", "Passwords do not match", "OK");
            return;
        }

        try
        {
            Console.WriteLine("Attempting to sign up with:");
            Console.WriteLine($"First Name: {FirstNameEntry.Text.Trim()}");
            Console.WriteLine($"Last Name: {LastNameEntry.Text.Trim()}");
            Console.WriteLine($"Email: {EmailEntry.Text.Trim()}");
            
            var response = await _apiService.SignUpAsync(
                FirstNameEntry.Text.Trim(),
                LastNameEntry.Text.Trim(),
                EmailEntry.Text.Trim(),
                PasswordEntry.Text,
                ConfirmPasswordEntry.Text
            );

            if (response.status == 200)
            {
                await DisplayAlert("Success", response.message ?? "Account created successfully", "OK");
                await Navigation.PopAsync(); // Go back to login page
            }
            else
            {
                await DisplayAlert("Error", response.message ?? "Failed to create account", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Sign-up error details: {ex}");
            await DisplayAlert("Error", $"An error occurred while creating your account: {ex.Message}", "OK");
        }
    }

    private async void OnLoginTapped(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

