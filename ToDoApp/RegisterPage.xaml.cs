using Microsoft.Maui.Controls;
using Microsoft.Maui.Networking;
using System.Diagnostics;
using ToDoApp.Services;

namespace ToDoApp
{
    public partial class RegisterPage : ContentPage
    {
        private readonly UserService _userService;
        private bool _isPasswordVisible = false;
        private bool _isConfirmPasswordVisible = false;

        public RegisterPage() : this(Application.Current.Handler.MauiContext.Services.GetService<UserService>()) {}

        public RegisterPage(UserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("No Internet", "Please check your internet connection and try again.", "OK");
                return;
            }

            var firstName = FirstNameEntry.Text;
            var lastName = LastNameEntry.Text;
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;
            var confirmPassword = ConfirmPasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(firstName))
            {
                await DisplayAlert("Error", "Please enter your first name", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                await DisplayAlert("Error", "Please enter your last name", "OK");
                return;
            }

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

            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                await DisplayAlert("Error", "Please confirm your password", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Error", "Passwords do not match", "OK");
                return;
            }

            try
            {
                RegisterButton.IsEnabled = false;

                var result = await _userService.SignUpAsync(firstName, lastName, email, password, confirmPassword);
                bool success = result.success;
                string message = result.message;

                Debug.WriteLine($"Registration response - Success: {success}, Message: {message}");

                if (success)
                {
                    await DisplayAlert("Success", "Account created successfully! Please sign in.", "OK");
                    await Shell.Current.GoToAsync("///LoginPage");
                }
                else
                {
                    await DisplayAlert("Registration Failed", message, "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error: {ex}");
                await DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
            }
            finally
            {
                RegisterButton.IsEnabled = true;
            }
        }

        private void OnPasswordToggleClicked(object sender, EventArgs e)
        {
            _isPasswordVisible = !_isPasswordVisible;
            PasswordEntry.IsPassword = !_isPasswordVisible;
            ((Button)sender).Text = _isPasswordVisible ? "🙈" : "👁️";
        }

        private void OnConfirmPasswordToggleClicked(object sender, EventArgs e)
        {
            _isConfirmPasswordVisible = !_isConfirmPasswordVisible;
            ConfirmPasswordEntry.IsPassword = !_isConfirmPasswordVisible;
            ((Button)sender).Text = _isConfirmPasswordVisible ? "🙈" : "👁️";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            FirstNameEntry.Text = string.Empty;
            LastNameEntry.Text = string.Empty;
            EmailEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
            ConfirmPasswordEntry.Text = string.Empty;
        }

        private async void OnSignInTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///LoginPage");
        }

        private async void OnGoogleSignUpClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Coming Soon", "Google Sign Up will be available soon!", "OK");
        }
    }
} 