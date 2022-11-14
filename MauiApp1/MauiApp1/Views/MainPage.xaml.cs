#if WINDOWS
using Microsoft.Maui.Platform;
using PInvoke;
using static PInvoke.User32;
using MicrosoftuiWindowing = Microsoft.UI.Windowing;
using MicrosoftuiXaml = Microsoft.UI.Xaml;
using Microsoftui = Microsoft.UI;
using MicrosoftuixmlMedia = Microsoft.UI.Xaml.Media;
using System.Reflection;
using MauiApp1.Extensions;
using MicrosoftuixamlData = Microsoft.UI.Xaml.Data;
#endif


namespace MauiApp1.Views;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
    }

#if WINDOWS

    private MicrosoftuiXaml.Thickness _NavigationViewContentMargin;
    public MicrosoftuiXaml.Thickness NavigationViewContentMargin
    {
        get => _NavigationViewContentMargin;
        set
        {
            _NavigationViewContentMargin = value;

            var mauiContext = this.RequireMauiContext();
            if (mauiContext is null)
                return;
             
            var windowRootView = mauiContext.GetNavigationRootManager().RootView as WindowRootView;
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
    }

    bool _IsBinding = false;

    bool SetBindingConfig()
    {
        var mauiContext = this.RequireMauiContext();
        if (mauiContext is null)
            return false;

        var windowRootView = mauiContext.GetNavigationRootManager().RootView as WindowRootView;
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
    private void Button_Clicked(object sender, EventArgs e)
	{
#if WINDOWS
        var winuiWindow = Window.Handler?.PlatformView as MicrosoftuiXaml.Window;
        if (winuiWindow is null)
            return;

        var windowHanlde = winuiWindow.GetWindowHandle();

        //var appWindow = winuiWindow.GetAppWindow();
        //if (appWindow is null)
            //return;
        //Windows.Graphics.RectInt32[] dragRects = dragRectsList.ToArray();
        //appWindow.TitleBar.SetDragRectangles()

        //winuiWindow.ExtendsContentIntoTitleBar = false;
        //_ = SetWindowPos(windowHanlde, SpecialWindowHandles.HWND_TOP,
        //                    0, 0, 800, 600,
        //                    SetWindowPosFlags.SWP_NOMOVE);

        //_ = SetWindowLong(windowHanlde,
        //   WindowLongIndexFlags.GWL_STYLE,
        //   (SetWindowLongFlags)(GetWindowLong(windowHanlde,
        //      WindowLongIndexFlags.GWL_STYLE) &
        //      ~(int)SetWindowLongFlags.WS_MINIMIZEBOX &
        //      ~(int)SetWindowLongFlags.WS_MAXIMIZEBOX));

        User32.PostMessage(windowHanlde, WindowMessage.WM_SYSCOMMAND, new IntPtr((int)SysCommands.SC_MAXIMIZE), IntPtr.Zero);
#endif
    }

    private void Button_Clicked_1(object sender, EventArgs e)
	{
#if WINDOWS
        var winuiWindow = Window.Handler?.PlatformView as MicrosoftuiXaml.Window;
        if (winuiWindow is null)
            return;

        var windowHanlde = winuiWindow.GetWindowHandle();
        User32.PostMessage(windowHanlde, WindowMessage.WM_SYSCOMMAND, new IntPtr((int)SysCommands.SC_MINIMIZE), IntPtr.Zero);
#endif
    }

    private void Button_Clicked_2(object sender, EventArgs e)
	{
#if WINDOWS

        var winuiWindow = Window.Handler?.PlatformView as MicrosoftuiXaml.Window;
        if (winuiWindow is null)
            return;
        var appWindow = winuiWindow.GetAppWindow();
        if (appWindow is null)
            return;

        var displyArea = MicrosoftuiWindowing.DisplayArea.Primary;
        double scalingFactor = winuiWindow.GetDisplayDensity();
        var width = 800 * scalingFactor;
        var height = 600 * scalingFactor;
        double startX = (displyArea.WorkArea.Width - width) / 2.0;
        double startY = (displyArea.WorkArea.Height - height) / 2.0;

        appWindow.MoveAndResize(new((int)startX, (int)startY, (int)width, (int)height), displyArea);

        //appWindow.TitleBar?.ResetToDefault();
#endif
    }

    private void Button_Clicked_3(object sender, EventArgs e)
    {
#if WINDOWS
        var winuiWindow = Window.Handler?.PlatformView as MicrosoftuiXaml.Window;
        if (winuiWindow is null)
            return;
        var appWindow = winuiWindow.GetAppWindow();
        if (appWindow is null)
            return;

        //ע������MauiĬ�Ͽ�������չTitleBar(�������ں�ģʽ��)������Ҫȥ�� ����ȫ����Ȼ����� �رհ�ť
        //��Ȼ�ر��˱������ں�ģʽ������ȫ��ʱ��Ȼ�����һ�����Ʊ������Ķ����������Ҫ������Ҫ������ȶ��ƣ����Բ鿴�ҵ�github��Ŀ��
        winuiWindow.Title = "MyTestApp";
        winuiWindow.ExtendsContentIntoTitleBar = false;
        appWindow.SetPresenter(MicrosoftuiWindowing.AppWindowPresenterKind.FullScreen);
#endif
    }

    private void Button_Clicked_4(object sender, EventArgs e)
    {
#if WINDOWS
        var winuiWindow = Window.Handler?.PlatformView as MicrosoftuiXaml.Window;
        if (winuiWindow is null)
            return;
        var appWindow = winuiWindow.GetAppWindow();
        if (appWindow is null)
            return;

        winuiWindow.ExtendsContentIntoTitleBar = true;
        appWindow.SetPresenter(MicrosoftuiWindowing.AppWindowPresenterKind.Default);

        var application = MicrosoftuiXaml.Application.Current;
        var res = application.Resources;
        res["WindowCaptionBackground"] = res["WindowCaptionBackgroundDisabled"];

        //�޸ı���������Ҫ����ˢ�²Ż���Ч
        TriggertTitleBarRepaint();

#endif
    }

    private void Button_Clicked_5(object sender, EventArgs e)
    {
#if WINDOWS
        var winuiWindow = Window.Handler?.PlatformView as MicrosoftuiXaml.Window;
        if (winuiWindow is null)
            return;
        var appWindow = winuiWindow.GetAppWindow();
        if (appWindow is null)
            return;

        var customOverlappedPresenter = MicrosoftuiWindowing.OverlappedPresenter.Create();
        appWindow.SetPresenter(customOverlappedPresenter);
        winuiWindow.ExtendsContentIntoTitleBar = false;
#endif
    }

    private void Button_Clicked_6(object sender, EventArgs e)
    {
#if WINDOWS
        var winuiWindow = Window.Handler?.PlatformView as MicrosoftuiXaml.Window;
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

    private void Button_Clicked_7(object sender, EventArgs e)
    {
#if WINDOWS
        var winuiWindow = Window.Handler?.PlatformView as MicrosoftuiXaml.Window;
        if (winuiWindow is null)
            return;

        var application = MicrosoftuiXaml.Application.Current;
        var res = application.Resources;

        //����������һ�����ɻ�Ϊʲô�����������������Ȥ�����Բ���Winui3��Դ��
        res["WindowCaptionBackground"] = new MicrosoftuixmlMedia.SolidColorBrush(Microsoftui.Colors.Red);

        //�޸ı���������Ҫ����ˢ�²Ż���Ч��������Ҫ����Ϊ����һ����С������
        TriggertTitleBarRepaint();
#endif
    }


    bool TriggertTitleBarRepaint()
    {
#if WINDOWS
        var winuiWindow = Window.Handler?.PlatformView as MicrosoftuiXaml.Window;
        if (winuiWindow is null)
            return false ;

        var windowHanlde = winuiWindow.GetWindowHandle();
        var activeWindow = User32.GetActiveWindow();
        if (windowHanlde == activeWindow)
        {
            User32.PostMessage(windowHanlde, WindowMessage.WM_ACTIVATE, new IntPtr((int)0x00), IntPtr.Zero);
            User32.PostMessage(windowHanlde, WindowMessage.WM_ACTIVATE, new IntPtr((int)0x01), IntPtr.Zero);
        }
        else
        {
            User32.PostMessage(windowHanlde, WindowMessage.WM_ACTIVATE, new IntPtr((int)0x01), IntPtr.Zero);
            User32.PostMessage(windowHanlde, WindowMessage.WM_ACTIVATE, new IntPtr((int)0x00), IntPtr.Zero);
        }

#endif
        return true;
    }
}