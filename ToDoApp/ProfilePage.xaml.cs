using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Diagnostics;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp
{
    [QueryProperty(nameof(UserId), "userId")]
    public partial class ProfilePage : ContentPage
    {
        private readonly ApiService _apiService;
        private string _userId;

        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                if (string.IsNullOrEmpty(_userId))
                {
                    _userId = Preferences.Get("UserId", string.Empty);
                }
                if (int.TryParse(_userId, out int userId))
                {
                    LoadUserData();
                }
            }
        }

        public ProfilePage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (string.IsNullOrEmpty(_userId))
            {
                _userId = Preferences.Get("UserId", string.Empty);
            }
            if (int.TryParse(_userId, out int userId))
            {
                LoadUserData();
            }
        }

        private async Task LoadUserData()
        {
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await DisplayAlert("Connection Issue", "Please check your internet connection and try again", "OK");
                    return;
                }

                // Load user data from preferences
                FirstNameLabel.Text = Preferences.Get("FirstName", string.Empty);
                LastNameLabel.Text = Preferences.Get("LastName", string.Empty);
                EmailLabel.Text = Preferences.Get("UserEmail", string.Empty);

                // Load task counts
                var activeTasksResponse = await _apiService.GetTodoItemsAsync("active", int.Parse(_userId));
                var completedTasksResponse = await _apiService.GetTodoItemsAsync("inactive", int.Parse(_userId));

                if (activeTasksResponse?.status == 200 && completedTasksResponse?.status == 200)
                {
                    ActiveTasksCountLabel.Text = activeTasksResponse.data?.Count.ToString() ?? "0";
                    CompletedTasksCountLabel.Text = completedTasksResponse.data?.Count.ToString() ?? "0";
                }
                else
                {
                    ActiveTasksCountLabel.Text = "0";
                    CompletedTasksCountLabel.Text = "0";
                    
                    string errorMessage = "Failed to load task statistics";
                    if (activeTasksResponse?.status == 401 || completedTasksResponse?.status == 401)
                    {
                        errorMessage = "Please login again";
                    }
                    await DisplayAlert("Error", errorMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Profile] Error: {ex}");
                await DisplayAlert("Error", "An error occurred while loading profile data", "OK");
            }
        }

        private async void OnEditProfileClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Coming Soon", "Edit profile functionality will be available soon!", "OK");
        }

        private async void OnChangePasswordClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Coming Soon", "Change password functionality will be available soon!", "OK");
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Logout", 
                "Are you sure you want to logout?", 
                "Yes", "No");

            if (confirm)
            {
                try
                {
                    // Clear stored user data
                    Preferences.Clear();
                    await SecureStorage.Default.SetAsync("auth_token", string.Empty);

                    // Navigate to login page using absolute path
                    await Shell.Current.GoToAsync("///LoginPage");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[Profile] Logout Error: {ex}");
                    await DisplayAlert("Error", "An error occurred while logging out", "OK");
                }
            }
        }
    }
}