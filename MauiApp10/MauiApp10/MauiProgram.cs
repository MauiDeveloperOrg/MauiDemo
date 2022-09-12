using CommunityToolkit.Maui;
using MauiApp10.ViewModels;
using MauiApp10.Views;

namespace MauiApp10;

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

    }

}
