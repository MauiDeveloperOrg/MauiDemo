using CommunityToolkit.Maui;
using MauiApp1.ViewModels;
using MauiApp1.Views;

namespace MauiApp1;

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

        return builder.Build();
    }

    static void RegisterViewAndViewModels(in IServiceCollection services)
    {
        services.AddTransient<MainPage, MainPageViewModel>();
    }

}
