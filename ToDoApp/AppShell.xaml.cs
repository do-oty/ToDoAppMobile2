using Microsoft.Maui.Controls;

namespace ToDoApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        RegisterRoutes();
    }

    private void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
        Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
        Routing.RegisterRoute(nameof(AddTaskPage), typeof(AddTaskPage));
        Routing.RegisterRoute(nameof(EditTaskPage), typeof(EditTaskPage));
        Routing.RegisterRoute(nameof(CompletedDashboardPage), typeof(CompletedDashboardPage));
        Routing.RegisterRoute(nameof(CompletedEditPage), typeof(CompletedEditPage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
    }
}
