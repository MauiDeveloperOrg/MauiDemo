

using MauiApp2.Events;
using MauiDesignControl.Extensions;

using Microsoft.Maui.Platform;

#if WINDOWS

using Microsoft.UI.Xaml;
using MauiDesignControl.Platforms.Windows;
#endif

namespace MauiApp1.Views;

[QueryProperty(nameof(TextString), "description")]
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        MessagingCenter.Subscribe<ParameterEvent>(this, nameof(ParameterEvent), p =>
        {
            TextString = p.Description;
        });

        Loaded += MainPage_Loaded;

    }

    private void MainPage_Loaded(object? sender, EventArgs e)
    {
        if (Window.Handler.PlatformView is not null)
        {
            Window_HandlerChanged(Window.Handler.PlatformView, EventArgs.Empty);
        }
        Window.HandlerChanged += Window_HandlerChanged;
    }

    private void Window_HandlerChanged(object? sender, EventArgs e)
    {
        var mauiContext = this.RequireMauiContext();
        if (mauiContext is null)
            return;

#if WINDOWS
        //var window = sender as Microsoft.UI.Xaml.Window;
        //if (window is null)
        //    return;
        //var resource = Microsoft.UI.Xaml.Application.Current.Resources;

        //resource.TryGetValue("AppTitleBar", out var titleBar);
        //if (titleBar is null)
        //    return;
        //window.ExtendsContentIntoTitleBar = false;
        //window.SetTitleBar(titleBar as UIElement);

        var windowRootView = mauiContext.GetNavigationRootManager().RootView as WindowRootView;
        if (windowRootView is null)
            return;

        var resource = Microsoft.UI.Xaml.Application.Current.Resources;
        resource.TryGetValue("AppTitleBarCustome", out var titleBar);

        windowRootView.AppTitleBarTemplate = titleBar as Microsoft.UI.Xaml.DataTemplate;


#endif
    }

    private string? _TextString;
    public string? TextString
    {
        get => _TextString;
        set
        {
            _TextString = value;
            PART_Label.Text = value;
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var dic = new Dictionary<string, object>();
        dic["parameter1"] = 100;

        await Shell.Current.GoToAsync("LoginRouter", true, dic);
    }
}