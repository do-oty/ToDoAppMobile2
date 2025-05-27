using Microsoft.Maui.Controls;
using ToDoApp.Services;

namespace ToDoApp;

public partial class AppShell : Shell
{
    private readonly UserService _userService;

    public AppShell(UserService userService)
    {
        InitializeComponent();
        _userService = userService;
        RegisterRoutes();
        
        // Always start with login page
        Device.BeginInvokeOnMainThread(async () => await GoToAsync("//LoginPage"));
    }

    private void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
        Routing.RegisterRoute(nameof(AddTaskPage), typeof(AddTaskPage));
        Routing.RegisterRoute(nameof(EditTaskPage), typeof(EditTaskPage));
        Routing.RegisterRoute(nameof(CompletedDashboardPage), typeof(CompletedDashboardPage));
        Routing.RegisterRoute(nameof(CompletedEditPage), typeof(CompletedEditPage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
    }
}
