namespace MauiDesignControl;

// All the code in this file is included in all platforms.
public static class AppHostExtesions
{
    public static MauiAppBuilder ConfigureCustomControls(this MauiAppBuilder builder, bool useCompatibilityRenderers = false)
    {
        builder
            .ConfigureMauiHandlers(handlers =>
            {
                 handlers.AddLibraryHandlers();
            });

        AddResourceLibrary();

        return builder;
    }

    public static IMauiHandlersCollection AddLibraryHandlers(this IMauiHandlersCollection handlers)
    {
        //handlers.AddTransient(typeof(CustomCheckBox), h => new CustomCheckBoxHandler());

        return handlers;
    }

    internal static bool AddResourceLibrary()
    {
#if WINDOWS
        var application = Microsoft.UI.Xaml.Application.Current;
        if (application == null)   
            return false;

        var resource = application.Resources;
        if (resource == null)
            return false;

        if (!resource.ContainsKey("MauiDesignControlIn"))
            resource.MergedDictionaries.Add(new Microsoft.UI.Xaml.ResourceDictionary { Source = new Uri("ms-appx:///MauiDesignControl/Platforms/Windows/Styles/Resources.xbf" )});

        if (!resource.ContainsKey("MauiDesignControlIn"))
            return false;
#endif

        return true;
    }
}
