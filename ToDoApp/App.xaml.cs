using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using ToDoApp.Services;

namespace ToDoApp;

public partial class App : Application
{
    private readonly UserService _userService;

    public App(UserService userService)
    {
        InitializeComponent();
        _userService = userService;
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnSleep()
    {
        base.OnSleep();
    }

    protected override void OnResume()
    {
        base.OnResume();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell(_userService));
        
        // Set any window-specific properties
        window.Title = "ToDoApp";
        
        return window;
    }
}