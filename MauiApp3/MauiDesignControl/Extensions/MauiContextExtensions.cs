namespace MauiDesignControl.Extensions;

public static class MauiContextExtensions
{
    public static bool IsApplicationOrNull(this object element) => element == null || element is IApplication;

    public static IEnumerable<Element> GetParentsPath(this Element self)
    {
        Element current = self;

        while (!current.RealParent.IsApplicationOrNull())
        {
            current = current.RealParent;
            yield return current;
        }
    }

    public static IMauiContext RequireMauiContext(this Element element, bool fallbackToAppMauiContext = false)
=> element.FindMauiContext(fallbackToAppMauiContext) ?? throw new InvalidOperationException($"{nameof(IMauiContext)} not found.");

    public static IMauiContext? FindMauiContext(this Element element, bool fallbackToAppMauiContext = false)
    {
        if (element is IElement fe && fe.Handler?.MauiContext != null)
            return fe.Handler.MauiContext;

        foreach (var parent in element.GetParentsPath())
        {
            if (parent is IElement parentView && parentView.Handler?.MauiContext != null)
                return parentView.Handler.MauiContext;
        }

        return fallbackToAppMauiContext ? Application.Current?.FindMauiContext() : default;
    }



}
