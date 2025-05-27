using ToDoApp.Services;

namespace ToDoApp;

public partial class ApiTestPage : ContentPage
{
    private readonly UserService _userService;

    public ApiTestPage(UserService userService)
    {
        InitializeComponent();
        _userService = userService;
    }

    private async void OnTestSignUpClicked(object sender, EventArgs e)
    {
        try
        {
            var (success, message) = await _userService.SignUpAsync(
                firstName: "Test",
                lastName: "User",
                email: "testuser@example.com",
                password: "123456",
                confirmPassword: "123456"
            );

            await DisplayAlert("Sign Up Test", 
                $"Success: {success}\nMessage: {message}", 
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", 
                $"An error occurred: {ex.Message}", 
                "OK");
        }
    }

    private async void OnTestSignInClicked(object sender, EventArgs e)
    {
        try
        {
            var (success, userData, message) = await _userService.SignInAsync(
                email: "testuser@example.com",
                password: "123456"
            );

            if (success && userData != null)
            {
                await DisplayAlert("Sign In Test", 
                    $"Success!\nUser ID: {userData.Id}\nName: {userData.FirstName} {userData.LastName}", 
                    "OK");
            }
            else
            {
                await DisplayAlert("Sign In Test", 
                    $"Failed: {message}", 
                    "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", 
                $"An error occurred: {ex.Message}", 
                "OK");
        }
    }
} 