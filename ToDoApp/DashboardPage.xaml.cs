using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp
{
    public partial class DashboardPage : ContentPage
    {
        private readonly TodoService _todoService;
        private ObservableCollection<TaskItem> _activeTasks;
        private string _userId;
        private bool _isRefreshing;

        public ObservableCollection<TaskItem> ActiveTasks => _activeTasks;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { _isRefreshing = value; OnPropertyChanged(); }
        }
        public ICommand RefreshCommand => new Command(async () => await LoadTasks());

        public DashboardPage()
        {
            InitializeComponent();
            _todoService = Application.Current.Handler.MauiContext.Services.GetService<TodoService>();
            _activeTasks = new ObservableCollection<TaskItem>();
            BindingContext = this;
            _userId = Preferences.Get("UserId", string.Empty);
            _ = LoadTasks();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (string.IsNullOrEmpty(_userId))
                _userId = Preferences.Get("UserId", string.Empty);
            _ = LoadTasks();
        }

        private async Task LoadTasks()
        {
            try
            {
                IsRefreshing = true;
                if (!int.TryParse(_userId, out int userId))
                {
                    await DisplayAlert("Error", "Invalid user ID", "OK");
                    return;
                }
                var (success, items, message) = await _todoService.GetTodoItemsAsync("active", userId);
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    _activeTasks.Clear();
                    if (success && items != null)
                    {
                        foreach (var item in items)
                        {
                            _activeTasks.Add(new TaskItem
                            {
                                Id = item.ItemId,
                                Title = item.ItemName,
                                Description = item.ItemDescription,
                                Status = item.Status,
                                UserId = item.UserId
                            });
                        }
                    }
                });
                if (!success)
                    await DisplayAlert("Error", message, "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dashboard] Error: {ex}");
                await DisplayAlert("Error", "Failed to load tasks", "OK");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async void OnAddTaskClicked(object sender, EventArgs e)
        {
            if (int.TryParse(_userId, out int userId))
            {
                await Navigation.PushAsync(new AddTaskPage(userId, async () => await LoadTasks(), _todoService));
            }
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is TaskItem taskItem)
            {
                await Navigation.PushAsync(new EditTaskPage(taskItem, async () => await LoadTasks(), _todoService));
            }
        }

        private void OnTaskCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox checkbox && checkbox.BindingContext is TaskItem taskItem)
            {
                if (e.Value)
                {
                    _ = HandleTaskCheckedAsync(taskItem, checkbox);
                }
            }
        }

        private async Task HandleTaskCheckedAsync(TaskItem taskItem, CheckBox checkbox)
        {
            var (success, message) = await _todoService.ChangeTodoStatusAsync("inactive", taskItem.Id);
            if (success)
            {
                ActiveTasks.Remove(taskItem);
            }
            else
            {
                await DisplayAlert("Error", message, "OK");
                checkbox.IsChecked = false;
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is TaskItem taskItem)
            {
                var confirm = await DisplayAlert("Delete Task", "Are you sure you want to delete this task?", "Yes", "No");
                if (confirm)
                {
                    var (success, message) = await _todoService.DeleteTodoItemAsync(taskItem.Id);
                    if (success)
                    {
                        ActiveTasks.Remove(taskItem);
                    }
                    else
                    {
                        await DisplayAlert("Error", message, "OK");
                    }
                }
            }
        }

        private async void OnCompleteClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is TaskItem taskItem)
            {
                var (success, message) = await _todoService.ChangeTodoStatusAsync("inactive", taskItem.Id);
                if (success)
                {
                    ActiveTasks.Remove(taskItem);
                }
                else
                {
                    await DisplayAlert("Error", message, "OK");
                }
            }
        }
    }
}