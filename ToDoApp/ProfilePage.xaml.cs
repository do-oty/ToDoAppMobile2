using Microsoft.Maui.Controls;

namespace ToDoApp;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
        if (answer)
        {
            // TODO: Implement logout logic
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
} 