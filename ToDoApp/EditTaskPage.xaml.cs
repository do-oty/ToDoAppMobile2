using Microsoft.Maui.Controls;
using System.Diagnostics;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp
{
    public partial class EditTaskPage : ContentPage
    {
        private readonly TodoService _todoService;
        private readonly TaskItem _taskItem;
        private readonly Func<Task> _refreshCallback;
        private bool _isLoading;

        public EditTaskPage(TaskItem taskItem, Func<Task> refreshCallback, TodoService todoService)
        {
            InitializeComponent();
            _todoService = todoService;
            _taskItem = taskItem;
            _refreshCallback = refreshCallback;
            BindingContext = _taskItem;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_taskItem.Title))
            {
                await DisplayAlert("Error", "Please enter a task title", "OK");
                return;
            }

            try
            {
                SetLoading(true);

                var (success, message) = await _todoService.UpdateTodoItemAsync(
                    _taskItem.Title,
                    _taskItem.Description,
                    _taskItem.Id
                );

                if (success)
                {
                    await DisplayAlert("Success", message, "OK");
                    await _refreshCallback();
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", message, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
            finally
            {
                SetLoading(false);
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnCompleteClicked(object sender, EventArgs e)
        {
            try
            {
                SetLoading(true);
                var (success, message) = await _todoService.ChangeTodoStatusAsync("inactive", _taskItem.Id);
                
                if (success)
                {
                    await DisplayAlert("Success", message, "OK");
                    await _refreshCallback();
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", message, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
            finally
            {
                SetLoading(false);
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Delete Task", 
                "Are you sure you want to delete this task?", 
                "Yes", "No");

            if (confirm)
            {
                try
                {
                    SetLoading(true);
                    var (success, message) = await _todoService.DeleteTodoItemAsync(_taskItem.Id);

                    if (success)
                    {
                        await DisplayAlert("Success", message, "OK");
                        await _refreshCallback();
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Error", message, "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                }
                finally
                {
                    SetLoading(false);
                }
            }
        }

        private async void OnTodoClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//DashboardPage");
        }

        private async void OnCompletedClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//CompletedDashboardPage?userId={Preferences.Get("UserId", string.Empty)}");
        }

        private async void OnProfileClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"///ProfilePage?userId={Preferences.Get("UserId", string.Empty)}");
        }

        private void SetLoading(bool isLoading)
        {
            _isLoading = isLoading;
            // LoadingIndicator.IsRunning = isLoading; // Removed, as LoadingIndicator no longer exists
        }
    }
}
