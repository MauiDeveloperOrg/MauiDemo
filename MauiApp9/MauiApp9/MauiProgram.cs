using CommunityToolkit.Maui;
using MauiApp9.Extensions;
using MauiApp9.ViewModels;
using MauiApp9.Views;

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
            })
            .UseMappers();

        RegisterViewAndViewModels(builder.Services);

        return builder.Build();
    }

    static void RegisterViewAndViewModels(in IServiceCollection services)
    {
        services.AddTransient<MainPage, MainPageViewModel>();
    }

}
