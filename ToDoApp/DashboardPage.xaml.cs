using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp
{
    [QueryProperty(nameof(UserId), "userId")]
    public partial class DashboardPage : ContentPage
    {
        private readonly ApiService _apiService;
        private ObservableCollection<TaskItem> _taskItems;
        private string _userId;

        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                // Load tasks when userId is set
                if (int.TryParse(_userId, out int userId))
                {
                    LoadTasks(userId);
                }
            }
        }

        public DashboardPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            _taskItems = new ObservableCollection<TaskItem>();
            TaskListView.ItemsSource = _taskItems;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Refresh when page appears
            if (int.TryParse(_userId, out int userId))
            {
                LoadTasks(userId);
            }
        }

        private async Task ShowLoading(bool show, string message = "Loading tasks...")
        {
            LoadingOverlay.IsVisible = show;
            LoadingIndicator.IsRunning = show;
            LoadingTextLabel.Text = message;

            // Disable interaction with the underlying content when loading
            TaskListView.IsEnabled = !show;
        }

        private async Task LoadTasks(int userId)
        {
            try
            {
                await ShowLoading(true);
                Debug.WriteLine($"[Dashboard] Loading tasks for user {userId}");

                // Check connectivity first
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    Debug.WriteLine("[Dashboard] No internet connection");
                    await ShowLoading(false);
                    await DisplayAlert("Connection Issue", "Please check your internet connection and try again", "OK");
                    return;
                }

                var response = await _apiService.GetTodoItemsAsync("all", userId);

                if (response?.status == 200 && response.data != null)
                {
                    Debug.WriteLine($"[Dashboard] Received {response.data.Count} tasks");
                    _taskItems.Clear();

                    foreach (var item in response.data.Values)
                    {
                        _taskItems.Add(new TaskItem
                        {
                            Id = item.item_id ?? 0,
                            Title = item.item_name ?? "Untitled Task",
                            Description = item.item_description ?? string.Empty,
                            Status = item.status ?? "active",
                            UserId = item.user_id ?? userId,
                            Time = DateTime.TryParse(item.timemodified, out var time)
                                ? time.ToString("h:mm tt")
                                : DateTime.Now.ToString("h:mm tt"),
                            Details = item.item_description ?? string.Empty,
                            Image = GetImageForStatus(item.status ?? "active")
                        });
                    }
                }
                else if (response != null)
                {
                    // Handle API error responses
                    string errorDetail = response.status switch
                    {
                        401 => "Please login again",
                        404 => "No tasks found",
                        500 => "Server error occurred",
                        _ => "Failed to load tasks"
                    };

                    Debug.WriteLine($"[Dashboard] API Error - Status: {response.status}");
                    await DisplayAlert("Error", errorDetail, "OK");
                }
                else
                {
                    Debug.WriteLine("[Dashboard] Empty API response");
                    await DisplayAlert("Error", "Received empty response from server", "OK");
                }
            }
            catch (HttpRequestException httpEx) when (httpEx.Message.Contains("404"))
            {
                Debug.WriteLine($"[Dashboard] 404 Error: {httpEx}");
                await DisplayAlert("Not Found", "The requested data couldn't be found", "OK");
            }
            catch (HttpRequestException httpEx)
            {
                Debug.WriteLine($"[Dashboard] Network Error: {httpEx}");
                await DisplayAlert("Connection Error", "Couldn't connect to the server. Please try again later.", "OK");
            }
            catch (JsonException jsonEx)
            {
                Debug.WriteLine($"[Dashboard] JSON Error: {jsonEx}");
                await DisplayAlert("Data Error", "There was a problem processing the response", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Dashboard] Unexpected Error: {ex}");
                await DisplayAlert("Error", "An unexpected error occurred", "OK");
            }
            finally
            {
                await ShowLoading(false);
            }
        }

        private string GetImageForStatus(string status)
        {
            return status.ToLower() switch
            {
                "completed" => "completed_task.jpg",
                "active" => "active_task.jpg",
                _ => "default_task.jpg"
            };
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            if (int.TryParse(_userId, out int userId))
            {
                await Navigation.PushAsync(new AddTaskPage(userId, async () =>
                {
                    Debug.WriteLine("[Dashboard] Refresh callback triggered");
                    await LoadTasks(userId);
                }));
            }
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var taskItem = button?.BindingContext as TaskItem;

            if (taskItem != null)
            {
                await Navigation.PushAsync(new EditTaskPage(taskItem));
            }
        }
    }
}