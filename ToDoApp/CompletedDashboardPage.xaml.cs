using System.Collections.ObjectModel;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp
{
    public partial class CompletedDashboardPage : ContentPage
    {
        private readonly ApiService _apiService;
        private ObservableCollection<TaskItem> _completedTasks;

        public CompletedDashboardPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            _completedTasks = new ObservableCollection<TaskItem>();

            _completedTasks.Add(new TaskItem
            {
                Id = 1,
                Title = "Cook dinner",
                Description = "maybe sausage",
                Time = "Yesterday, 3:00 PM",
                Image = "cook.JPG"
            });

            _completedTasks.Add(new TaskItem
            {
                Id = 2,
                Title = "Midterms",
                Description = "Para pasar!!",
                Time = "Yesterday, 10:00 AM",
                Image = "study.JPG"
            });

            _completedTasks.Add(new TaskItem
            {
                Id = 3,
                Title = "Laundry",
                Description = "Remember To Separate the colors",
                Time = "Monday, 2:00 PM",
                Image = "laundry.jpg"
            });

            CompletedTaskListView.ItemsSource = _completedTasks;
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton button && button.CommandParameter is TaskItem task)
            {
                await Navigation.PushAsync(new CompletedEditPage(task));
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton button && button.CommandParameter is TaskItem task)
            {
                bool answer = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this task?", "Yes", "No");
                if (answer)
                {
                    _completedTasks.Remove(task);
                    await DisplayAlert("Success", "Task deleted successfully", "OK");
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
