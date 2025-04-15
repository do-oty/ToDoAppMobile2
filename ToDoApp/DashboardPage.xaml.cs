using ToDoApp.Models;

namespace ToDoApp
{
    public partial class DashboardPage : ContentPage
    {
        private List<TaskItem> taskList;

        public DashboardPage()
        {
            InitializeComponent();
            taskList = new List<TaskItem>
            {
                new TaskItem
                {
                    Title = "Complete Project Proposal",
                    Time = "10:00 AM",
                    Image = "coding.JPG",
                    Description = "Finish the project proposal document",
                    Details = "Include budget estimates and timeline"
                },
                new TaskItem
                {
                    Title = "Afternoon exercise",
                    Time = "2:00 PM",
                    Image = "exercise.JPG",
                    Description = "Exercise",
                    Details = "Exercise afternoon"
                }
            };
            TaskListView.ItemsSource = taskList;
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTaskPage());
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
