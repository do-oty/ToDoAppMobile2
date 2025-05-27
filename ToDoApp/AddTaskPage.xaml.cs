using System.Diagnostics;
using ToDoApp.Services;

namespace ToDoApp
{
    public partial class AddTaskPage : ContentPage
    {
        private readonly int _userId;
        private readonly TodoService _todoService;
        private readonly Func<Task> _refreshCallback;

        public AddTaskPage(int userId, Func<Task> refreshCallback, TodoService todoService)
        {
            InitializeComponent();
            _userId = userId;
            _todoService = todoService;
            _refreshCallback = refreshCallback;
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleEntry.Text))
            {
                await DisplayAlert("Error", "Please enter a title for the task", "OK");
                return;
            }

            try
            {
                var (success, item, message) = await _todoService.AddTodoItemAsync(
                    TitleEntry.Text,
                    DescriptionEditor.Text,
                    _userId
                );

                if (success)
                {
                    await DisplayAlert("Success", message, "OK");
                    if (_refreshCallback != null)
                    {
                        await _refreshCallback();
                    }
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", message, "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AddTask] Error: {ex}");
                await DisplayAlert("Error", "An error occurred while adding the task", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}