using System.Collections.ObjectModel;
using System.Diagnostics;
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
            CompletedTaskListView.ItemsSource = _completedTasks;

            // Start loading tasks
            LoadCompletedTasks();
        }

        private async Task ShowLoading(bool show, string message = "Loading completed tasks...")
        {
            LoadingOverlay.IsVisible = show;
            LoadingIndicator.IsRunning = show;
            LoadingTextLabel.Text = message;

            // Disable interaction with the underlying content when loading
            CompletedTaskListView.IsEnabled = !show;
        }

        private async void LoadCompletedTasks()
        {
            try
            {
                await ShowLoading(true);

                // Simulate loading delay (remove this in production)
                await Task.Delay(1000);

                // Clear existing items
                _completedTasks.Clear();

                // Add your tasks (replace with actual API call)
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

                Debug.WriteLine("Completed tasks loaded successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading completed tasks: {ex.Message}");
                await DisplayAlert("Error", "Failed to load completed tasks", "OK");
            }
            finally
            {
                await ShowLoading(false);
            }
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is TaskItem task)
            {
                await Navigation.PushAsync(new CompletedEditPage(task));
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is TaskItem task)
            {
                bool answer = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this task?", "Yes", "No");
                if (answer)
                {
                    await ShowLoading(true, "Deleting task...");
                    try
                    {
                        // Simulate deletion delay
                        await Task.Delay(500);
                        _completedTasks.Remove(task);
                        await DisplayAlert("Success", "Task deleted successfully", "OK");
                    }
                    finally
                    {
                        await ShowLoading(false);
                    }
                }
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Refresh when page appears
            // await LoadCompletedTasks(); Make this functional once the backend function works
        }
    }
}