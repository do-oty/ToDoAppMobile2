using System.Diagnostics;
using System.Text.Json;
using ToDoApp.Services;

namespace ToDoApp
{
    public partial class AddTaskPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly int _userId;
        private readonly Func<Task> _refreshCallback;

        public AddTaskPage(int userId, Func<Task> refreshCallback)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _userId = userId;
            _refreshCallback = refreshCallback;
            Debug.WriteLine($"[AddTaskPage] Initialized for user {userId}");
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TitleEntry.Text))
                {
                    await DisplayAlert("Error", "Please enter a title", "OK");
                    return;
                }

                // Show loading state
                LoadingIndicator.IsVisible = true;
                AddButton.IsEnabled = false;
                Debug.WriteLine($"[AddTask] Attempting to add task for user {_userId}");

                var response = await _apiService.AddTodoItemAsync(
                    TitleEntry.Text.Trim(),
                    DetailsEditor.Text?.Trim() ?? string.Empty,
                    _userId
                );

                if (response?.status == 200)
                {
                    Debug.WriteLine($"[AddTask] Success! Item ID: {response.data?.item_id}");

                    // Call the refresh callback before navigating back
                    if (_refreshCallback != null)
                    {
                        Debug.WriteLine("[AddTask] Executing refresh callback");
                        await _refreshCallback();
                    }

                    await DisplayAlert("Success", "Task added successfully", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    var errorMsg = response?.message ?? "The server returned an unexpected response";
                    Debug.WriteLine($"[AddTask] Error: {errorMsg}");
                    await DisplayAlert("Error", errorMsg, "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AddTask] CRITICAL ERROR: {ex}\n{ex.StackTrace}");
                await DisplayAlert("Error",
                    ex.InnerException?.Message ?? ex.Message,
                    "OK");
            }
            finally
            {
                LoadingIndicator.IsVisible = false;
                AddButton.IsEnabled = true;
                Debug.WriteLine("[AddTask] Reset UI state");
            }
        }
    }
}