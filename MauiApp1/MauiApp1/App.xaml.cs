#if WINDOWS
using MauiApp1.Extensions;
using System.Reflection;
using MicrosoftuiWindowing = Microsoft.UI.Windowing;
using MicrosoftuiXaml = Microsoft.UI.Xaml;
using MicrosoftuixamlData = Microsoft.UI.Xaml.Data;
using Windowsgraphics = Windows.Graphics;
using Windowsfoundation = Windows.Foundation;
using Microsoft.Maui.Platform;
using System.Runtime.InteropServices; 
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

        //开启无边框应用
        winuiWindow.ExtendsContentIntoTitleBar = false;
        var customOverlappedPresenter = MicrosoftuiWindowing.OverlappedPresenter.CreateForContextMenu();
        appWindow.SetPresenter(customOverlappedPresenter);

        if (!_IsBinding)
            SetBindingConfig();
        NavigationViewContentMargin = new MicrosoftuiXaml.Thickness(0);

        {
            //开启可拖动功能 改功能能够正常运行的条件是 ExtendsContentIntoTitleBar = true
            //这里需要自己排除可点击区域部分的的功能 比如当你去掉标题栏后 你自己实现了一个自定义标题栏 标题栏上有按钮 你需要自行排除可点击区域
            //排除可点击区域会面临 界面缩放后 坐标发生变化导致点击区域移动 你需要自行处理
            //排除可点击区域会面临 dpi发生变化后 坐标发生变化导致点击区域移动 你需要自行处理 

            //本例简单的认为 自定义标题栏 高40 宽度是界面打开默认宽度 不做任何处理
            List<Windowsgraphics.RectInt32> rectInt32s = new();
            var rectInt32 = new Windowsgraphics.RectInt32(0, 0, appWindow.Size.Width, 40);
            rectInt32s.Add(rectInt32);
            appWindow.TitleBar.SetDragRectangles(rectInt32s.ToArray());
        }

        {
            //自定义实现可拖动窗口功能
        }

        _MauiWindow = window;
        _AppWindow = appWindow;
        winuiWindow.Content.PointerPressed += Content_PointerPressed;
        winuiWindow.Content.PointerReleased += Content_PointerReleased;
        winuiWindow.Content.PointerMoved += Content_PointerMoved;
#endif
    }

#if WINDOWS

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct POINT
    {
        public int x;
        public int y;

        public static implicit operator Point(POINT point)
        {
            return new Point(point.x, point.y);
        }

        public static implicit operator POINT(Windowsfoundation.Point point)
        {
            POINT result = default(POINT);
            result.x = (int)point.X;
            result.y = (int)point.Y;
            return result;
        }
    }

    [DllImport("User32", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool GetCursorPos(out POINT lpPoint);

    public static POINT GetCursorPos()
    {
        POINT result = default(POINT);
        GetCursorPos(out result);
        return result;
    }

    Window _MauiWindow = default;
    MicrosoftuiWindowing.AppWindow _AppWindow = default;

    bool _IsDragStarting = false;
    POINT _PointStaring = default;
    double _X = 0;
    double _Y = 0;

    private void Content_PointerMoved(object sender, MicrosoftuiXaml.Input.PointerRoutedEventArgs e)
    {
        if (!_IsDragStarting)
            return;

        if (_AppWindow is null)
            return;

        var point = GetCursorPos(); 
        //var vInt32Point = new Windowsgraphics.PointInt32(point.x, point.y);
        //var displyArea = MicrosoftuiWindowing.DisplayArea.GetFromPoint(vInt32Point, MicrosoftuiWindowing.DisplayAreaFallback.None);

        var x = _X + point.x - _PointStaring.x;
        var y = _Y + point.y - _PointStaring.y;
        var pointInt32 = new Windowsgraphics.PointInt32((int)x, (int)y);

        _AppWindow.Move(pointInt32);
    }

    private void Content_PointerReleased(object sender, MicrosoftuiXaml.Input.PointerRoutedEventArgs e)
    {
        _IsDragStarting = false;
        _PointStaring = default;
    }

    private void Content_PointerPressed(object sender, MicrosoftuiXaml.Input.PointerRoutedEventArgs e)
    {
        if (sender is not MicrosoftuiXaml.UIElement uiElement)
            return;

        var point = e.GetCurrentPoint(uiElement);
        if (point.Position.Y > 40) // 假设标题栏Height = 40
            return;

        var point1 = GetCursorPos();

        _IsDragStarting = true;
        _PointStaring = point1;

        _X = _MauiWindow.X;
        _Y = _MauiWindow.Y;
    }
#endif
}
