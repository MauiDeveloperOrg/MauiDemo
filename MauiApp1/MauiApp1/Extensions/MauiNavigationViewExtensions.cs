using Microsoft.Maui.Platform;

namespace MauiApp1.Extensions;
public static class MauiNavigationViewExtensions
{

#if WINDOWS
    public static NavigationRootManager GetNavigationRootManager(this IMauiContext mauiContext) => mauiContext.Services.GetRequiredService<NavigationRootManager>();

#endif
}
