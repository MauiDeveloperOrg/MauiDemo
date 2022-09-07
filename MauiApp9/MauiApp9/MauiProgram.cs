using CommunityToolkit.Maui;
using MauiApp9.ViewModels;
using MauiApp9.Views;
using Plugin.Maui.Audio;

namespace MauiApp9;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });


        RegisterViewAndViewModels(builder.Services);
        UseMauiExtensions(builder.Services);

        return builder.Build();
    }

    static void RegisterViewAndViewModels(in IServiceCollection services)
    {
        services.AddTransient<MainPage, MainPageViewModel>();
    }

    static void UseMauiExtensions(in IServiceCollection services)
    {
        services.AddSingleton<IAudioManager>(AudioManager.Current);
    }

}
