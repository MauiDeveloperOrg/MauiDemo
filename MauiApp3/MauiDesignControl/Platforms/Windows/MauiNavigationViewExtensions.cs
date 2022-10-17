using Microsoft.Maui.Platform;

namespace MauiDesignControl.Platforms.Windows;

public static class MauiNavigationViewExtensions
{
    public static NavigationRootManager GetNavigationRootManager(this IMauiContext mauiContext) => mauiContext.Services.GetRequiredService<NavigationRootManager>();

}
