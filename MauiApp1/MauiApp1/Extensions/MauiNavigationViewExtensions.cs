using Microsoft.Maui.Platform;

namespace MauiApp1.Extensions;
public static class MauiNavigationViewExtensions
{

#if WINDOWS
    public static NavigationRootManager? GetNavigationRootManager(this IMauiContext mauiContext)
    {
        var rootManager = mauiContext.Services.GetService<NavigationRootManager>();
        return rootManager;
    }

#endif
}
