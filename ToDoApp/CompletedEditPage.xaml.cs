using Microsoft.Maui.Controls;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp
{
    public partial class CompletedEditPage : ContentPage
    {
        private readonly TaskItem _task;
        private readonly ApiService _apiService;

        public CompletedEditPage(TaskItem taskItem)
        {
            InitializeComponent();
            _task = taskItem;
            _apiService = new ApiService();

            TitleEntry.Text = _task.Title;
            DescriptionEditor.Text = _task.Description;
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TitleEntry.Text))
                {
                    await DisplayAlert("Error", "Please enter a title for the task", "OK");
                    return;
                }

                var response = await _apiService.UpdateTodoItemAsync(
                    TitleEntry.Text.Trim(),
                    DescriptionEditor.Text?.Trim() ?? "",
                    _task.Id
                );

                if (response.status == 200)
                {
                    _task.Title = TitleEntry.Text.Trim();
                    _task.Description = DescriptionEditor.Text?.Trim() ?? "";
                    await DisplayAlert("Success", "Task updated successfully!", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    Console.WriteLine($"Failed to update task: {response.status}");
                    await DisplayAlert("Error", "Failed to update task. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating task: {ex.Message}");
                await DisplayAlert("Error", "An error occurred while updating the task. Please check your internet connection and try again.", "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            try
            {
                bool confirm = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this task?", "Yes", "No");
                if (!confirm) return;

                var response = await _apiService.DeleteTodoItemAsync(_task.Id);

                if (response?.status == 200)
                {
                    await DisplayAlert("Success", "Task deleted successfully!", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    Console.WriteLine($"Failed to delete task: {response?.message}");
                    await DisplayAlert("Error", "Failed to delete task. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting task: {ex.Message}");
                await DisplayAlert("Error", "An error occurred while deleting the task. Please check your internet connection and try again.", "OK");
            }
        }
    }
}
