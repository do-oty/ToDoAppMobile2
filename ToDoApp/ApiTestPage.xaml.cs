using ToDoApp.Services;

namespace ToDoApp;

public partial class ApiTestPage : ContentPage
{
    private readonly ApiService _apiService;

    public ApiTestPage()
    {
        InitializeComponent();
        _apiService = new ApiService();
    }

    private async void OnTestSignUpClicked(object sender, EventArgs e)
    {
        try
        {
            var response = await _apiService.SignUpAsync(
                firstName: "Test",
                lastName: "User",
                email: "testuser@example.com",
                password: "123456",
                confirmPassword: "123456"
            );

            await DisplayAlert("Sign Up Test", 
                $"Status: {response.status}\nMessage: {response.message}", 
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
            var response = await _apiService.SignInAsync(
                email: "testuser@example.com",
                password: "123456"
            );

            if (response.status == 200)
            {
                await DisplayAlert("Sign In Test", 
                    $"Success!\nUser ID: {response.data.id}\nName: {response.data.first_name} {response.data.last_name}", 
                    "OK");
            }
            else
            {
                await DisplayAlert("Sign In Test", 
                    $"Failed: {response.message}", 
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