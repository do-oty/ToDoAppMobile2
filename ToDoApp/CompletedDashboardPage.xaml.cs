using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp
{
    [QueryProperty(nameof(UserId), "userId")]
    public partial class CompletedDashboardPage : ContentPage
    {
        private readonly TodoService _todoService;
        private ObservableCollection<TaskItem> _completedTasks;
        private string _userId;
        private bool _isRefreshing;

        public ObservableCollection<TaskItem> CompletedTasks => _completedTasks;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { _isRefreshing = value; OnPropertyChanged(); }
        }
        public ICommand RefreshCommand => new Command(async () => await LoadTasks());

        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged();
                if (string.IsNullOrEmpty(_userId))
                {
                    _userId = Preferences.Get("UserId", string.Empty);
                }
                Debug.WriteLine($"[CompletedDashboard] UserId set to: {_userId}");
                if (int.TryParse(_userId, out int userId))
                {
                    _ = LoadTasks();
                }
            }
        }

        public CompletedDashboardPage()
        {
            InitializeComponent();
            _todoService = Application.Current.Handler.MauiContext.Services.GetService<TodoService>();
            _completedTasks = new ObservableCollection<TaskItem>();
            BindingContext = this;
            _userId = Preferences.Get("UserId", string.Empty);
            _ = LoadTasks();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (string.IsNullOrEmpty(_userId))
            {
                _userId = Preferences.Get("UserId", string.Empty);
                Debug.WriteLine($"[CompletedDashboard] UserId from preferences in OnAppearing: {_userId}");
            }
            if (int.TryParse(_userId, out int userId))
            {
                _ = LoadTasks();
            }
        }

        private async Task LoadTasks()
        {
            try
            {
                IsRefreshing = true;
                Debug.WriteLine($"[CompletedDashboard] Loading tasks for user {_userId}");

                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    Debug.WriteLine("[CompletedDashboard] No internet connection");
                    await DisplayAlert("Connection Issue", "Please check your internet connection and try again", "OK");
                    return;
                }

                if (!int.TryParse(_userId, out int userId))
                {
                    Debug.WriteLine($"[CompletedDashboard] Invalid user ID: {_userId}");
                    await DisplayAlert("Error", "Invalid user ID", "OK");
                    return;
                }

                Debug.WriteLine($"[CompletedDashboard] Fetching completed tasks for user ID: {userId}");
                var (success, items, message) = await _todoService.GetTodoItemsAsync("inactive", userId);

                Debug.WriteLine($"[CompletedDashboard] Completed tasks response - Success: {success}, Count: {items?.Count}, Message: {message}");

                if (success)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        _completedTasks.Clear();
                        foreach (var item in items)
                        {
                            Debug.WriteLine($"[CompletedDashboard] Adding task - ID: {item.ItemId}, Name: {item.ItemName}");
                            _completedTasks.Add(new TaskItem
                            {
                                Id = item.ItemId,
                                Title = item.ItemName,
                                Description = item.ItemDescription,
                                Status = item.Status,
                                UserId = item.UserId,
                                Time = DateTime.TryParse(item.TimeModified, out var time)
                                    ? time.ToString("h:mm tt")
                                    : DateTime.Now.ToString("h:mm tt")
                            });
                        }
                        OnPropertyChanged(nameof(CompletedTasks));
                    });
                }
                else
                {
                    Debug.WriteLine($"[CompletedDashboard] Error loading tasks: {message}");
                    await DisplayAlert("Error", message, "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CompletedDashboard] Error: {ex}");
                await DisplayAlert("Error", "An unexpected error occurred", "OK");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is TaskItem taskItem)
            {
                await Navigation.PushAsync(new EditTaskPage(taskItem, async () => await LoadTasks(), _todoService));
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is TaskItem taskItem)
            {
                var confirm = await DisplayAlert("Delete Task", "Are you sure you want to delete this task?", "Yes", "No");
                if (confirm)
                {
                    try
                    {
                        var (success, message) = await _todoService.DeleteTodoItemAsync(taskItem.Id);
                        if (success)
                        {
                            CompletedTasks.Remove(taskItem);
                            await DisplayAlert("Success", message, "OK");
                        }
                        else
                        {
                            await DisplayAlert("Error", message, "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[CompletedDashboard] Delete Error: {ex}");
                        await DisplayAlert("Error", "An error occurred while deleting the task", "OK");
                    }
                }
            }
        }

        private void OnRestoreCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox checkbox && checkbox.BindingContext is TaskItem taskItem && e.Value)
            {
                _ = HandleRestoreCheckedAsync(taskItem, checkbox);
            }
        }

        private async Task HandleRestoreCheckedAsync(TaskItem taskItem, CheckBox checkbox)
        {
            var (success, message) = await _todoService.ChangeTodoStatusAsync("active", taskItem.Id);
            if (success)
            {
                CompletedTasks.Remove(taskItem);
            }
            else
            {
                await DisplayAlert("Error", message, "OK");
                checkbox.IsChecked = false;
            }
        }

        private async void OnAddTaskClicked(object sender, EventArgs e)
        {
            if (int.TryParse(_userId, out int userId))
            {
                await Navigation.PushAsync(new AddTaskPage(userId, async () => await LoadTasks(), _todoService));
            }
        }

        private async void OnRestoreClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is TaskItem taskItem)
            {
                var (success, message) = await _todoService.ChangeTodoStatusAsync("active", taskItem.Id);
                if (success)
                {
                    CompletedTasks.Remove(taskItem);
                }
                else
                {
                    await DisplayAlert("Error", message, "OK");
                }
            }
        }
    }
}