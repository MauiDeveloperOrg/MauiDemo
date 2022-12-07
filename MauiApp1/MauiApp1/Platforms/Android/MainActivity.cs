using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Maui.Platform;

namespace MauiApp1;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        //全屏
        Window?.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

        //半透明任务栏
        //Window?.SetFlags(WindowManagerFlags.TranslucentStatus, WindowManagerFlags.TranslucentStatus);

        //全透明任务栏
        //Window?.SetFlags(WindowManagerFlags.TranslucentNavigation, WindowManagerFlags.TranslucentNavigation);

        //设置状态烂 导航栏颜色为透明
        Window?.SetStatusBarColor(Colors.Transparent.ToPlatform());
        Window?.SetNavigationBarColor(Colors.Transparent.ToPlatform());


        base.OnCreate(savedInstanceState);
    }
}
