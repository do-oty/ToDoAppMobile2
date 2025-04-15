using Microsoft.Maui.Controls;
using ToDoApp.Models;

namespace ToDoApp
{
    public partial class EditTaskPage : ContentPage
    {
        private TaskItem task;

        public EditTaskPage(TaskItem taskItem)
        {
            InitializeComponent();
            task = taskItem;
            TitleEntry.Text = task.Title;
            DetailsEditor.Text = task.Details;
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnUpdateClicked(object sender, EventArgs e)
        {
            task.Title = TitleEntry.Text;
            task.Details = DetailsEditor.Text;
            await DisplayAlert("Updated", "Task updated successfully!", "OK");
            await Navigation.PopAsync();
        }

        private async void OnCompleteClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Completed", "Task marked as complete!", "OK");
            await Navigation.PopAsync();
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Deleted", "Task deleted!", "OK");
            await Navigation.PopAsync();
        }

        private async void OnTodoClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//DashboardPage");
        }

        private async void OnCompletedClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//CompletedDashboardPage");
        }

        private async void OnProfileClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//SignUpPage");
        }
    }
}
