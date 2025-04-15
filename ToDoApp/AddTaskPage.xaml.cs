using Microsoft.Maui.Controls;

namespace ToDoApp
{
    public partial class AddTaskPage : ContentPage
    {
        public AddTaskPage()
        {
            InitializeComponent();
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            string title = TitleEntry.Text;
            string details = DetailsEditor.Text;

            // Here you can handle saving the task to your task list

            await DisplayAlert("Task Added", $"Title: {title}", "OK");
            await Navigation.PopAsync(); // Navigate back to Dashboard
        }
    }
}
