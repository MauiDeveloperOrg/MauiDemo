#if WINDOWS
using MauiApp1.Extensions;
using Microsoft.Maui.Platform;
using System.Reflection;
using MicrosoftuiWindowing = Microsoft.UI.Windowing;
using MicrosoftuiXaml = Microsoft.UI.Xaml;
using MicrosoftuixamlData = Microsoft.UI.Xaml.Data;
#endif



namespace MauiApp1;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }


#if WINDOWS

    private MicrosoftuiXaml.Thickness _NavigationViewContentMargin;
    public MicrosoftuiXaml.Thickness NavigationViewContentMargin
    {
        get => _NavigationViewContentMargin;
        set
        {
            _NavigationViewContentMargin = value;

            var page = Windows.FirstOrDefault()?.Page;
            if (page is null)
                return;
            var mauiContext = page.RequireMauiContext();
            if (mauiContext is null)
                return;

            try
            {
                var windowRootView = mauiContext.GetNavigationRootManager()?.RootView as WindowRootView;
                var navigationView = windowRootView?.NavigationViewControl;
                if (navigationView is null)
                    return;

                var thicknessProperty = typeof(RootNavigationView).GetProperty("NavigationViewContentMargin", BindingFlags.Instance | BindingFlags.NonPublic);
                if (thicknessProperty?.GetValue(navigationView) is MicrosoftuiXaml.Thickness thickness)
                {
                    if (thickness == new MicrosoftuiXaml.Thickness(0))
                        return;

                    thicknessProperty.SetValue(navigationView, new MicrosoftuiXaml.Thickness(0));
                }
            }
            catch (Exception)
            {
                return;
            }

           
        }
    }

    bool _IsBinding = false;

    bool SetBindingConfig()
    {
        var page = Windows.FirstOrDefault()?.Page;
        if (page is null)
            return false;
        
        var mauiContext = page.RequireMauiContext();
        if (mauiContext is null)
            return false;

        var windowRootView = mauiContext.GetNavigationRootManager()?.RootView as WindowRootView;
        var navigationView = windowRootView?.NavigationViewControl;
        if (navigationView is null)
            return false;

        var contentProperty = typeof(RootNavigationView).GetProperty("ContentGrid", BindingFlags.Instance | BindingFlags.NonPublic);
        if (contentProperty is null)
            return false;

        var contentGrid = contentProperty.GetValue(navigationView);
        if (contentGrid is not MicrosoftuiXaml.FrameworkElement frameworkELement)
            return false;

        MicrosoftuixamlData.Binding marginBinding = new();
        marginBinding.Source = this;
        marginBinding.Path = new MicrosoftuiXaml.PropertyPath("NavigationViewContentMargin");
        marginBinding.Mode = MicrosoftuixamlData.BindingMode.TwoWay;
        marginBinding.UpdateSourceTrigger = MicrosoftuixamlData.UpdateSourceTrigger.PropertyChanged;
        MicrosoftuixamlData.BindingOperations.SetBinding(frameworkELement, MicrosoftuiXaml.FrameworkElement.MarginProperty, marginBinding);

        return true;
    }

#endif


    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

        window.HandlerChanged += Window_HandlerChanged;

        return window;
    }

    private void Window_HandlerChanged(object? sender, EventArgs e)
    {
        if (sender is not Window window)
            return;

#if WINDOWS
        var winuiWindow = window.Handler?.PlatformView as MicrosoftuiXaml.Window;
        if (winuiWindow is null)
            return;
 
        var appWindow = winuiWindow.GetAppWindow();
        if (appWindow is null)
            return;

        winuiWindow.ExtendsContentIntoTitleBar = false;
        var customOverlappedPresenter = MicrosoftuiWindowing.OverlappedPresenter.CreateForContextMenu();
        appWindow.SetPresenter(customOverlappedPresenter);

        if (!_IsBinding)
            SetBindingConfig();

        NavigationViewContentMargin = new MicrosoftuiXaml.Thickness(0);
#endif
    }
}
