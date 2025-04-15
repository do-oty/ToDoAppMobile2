using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;

namespace ToDoApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());
        
        // Set any window-specific properties
        window.Title = "ToDoApp";
        
        return window;
    }
}