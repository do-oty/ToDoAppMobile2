namespace ToDoApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void TogglePasswordVisibility(object sender, EventArgs e)
        {
            PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
            PasswordToggleButton.Source = PasswordEntry.IsPassword ? "eyebrow.svg" : "eyeopen.svg";
        }


        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///SignUpPage");

        }
    }
}
