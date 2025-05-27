using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Diagnostics;

namespace ToDoApp
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            
            // Start loading immediately when page appears
            this.Appearing += async (s, e) => await LoadUserProfile();
        }

        private async Task LoadUserProfile()
        {
            try
            {
                // Show loading indicator
                LoadingOverlay.IsVisible = true;
                LoadingIndicator.IsRunning = true;
                this.IsEnabled = false; // Disable interaction during load

                Debug.WriteLine("[Profile] Loading user data...");
                
                // Retrieve stored user data
                var firstName = Preferences.Get("FirstName", "First");
                var lastName = Preferences.Get("LastName", "Last");

                var email = Preferences.Get("UserEmail", "email@example.com");

                // Simulate network delay (remove in production)
                await Task.Delay(800); 

                // Update UI
                UserEmailLabel.Text = email;
                ProfileImage.Source = "profile.png";

                Debug.WriteLine("[Profile] Loaded successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Profile] Error: {ex}");
                await DisplayAlert("Error", "Failed to load profile data", "OK");
            }
            finally
            {
                LoadingOverlay.IsVisible = false;
                LoadingIndicator.IsRunning = false;
                this.IsEnabled = true;
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
            if (answer)
            {
                try
                {
                    // Show loading overlay and change text
                    LoadingTextLabel.Text = "Logging out...";
                    LoadingOverlay.IsVisible = true;
                    LoadingIndicator.IsRunning = true;
                    this.IsEnabled = false;

                    // Optional delay to show the message clearly
                    await Task.Delay(500);

                    // Clear stored user preferences
                    Preferences.Remove("UserId");
                    Preferences.Remove("UserName");
                    Preferences.Remove("UserEmail");
                    Preferences.Remove("FirstName");
                    Preferences.Remove("LastName");

                    // Navigate to login page
                    await Shell.Current.GoToAsync("//LoginPage");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[Logout] Error: {ex}");
                    await DisplayAlert("Error", "An error occurred during logout", "OK");
                }
                finally
                {
                    // Reset for safety, though app will likely navigate away
                    LoadingTextLabel.Text = "Loading profile...";
                    LoadingOverlay.IsVisible = false;
                    LoadingIndicator.IsRunning = false;
                    this.IsEnabled = true;
                }
            }
        }

    }
}