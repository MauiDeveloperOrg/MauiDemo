using MauiApp9.Handlers;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace MauiApp9.Mappers;
public static class EntryHandlerMapper
{
    public static bool AppendToMpping()
    {
        EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
            if (view is not BorderlessEntry)
                return;
#if __ANDROID__

            handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());

#elif __IOS__ || MACCATALYST

           handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;

#elif WINDOWS

            handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
            handler.PlatformView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;
            handler.PlatformView.Background = Colors.Transparent.ToPlatform();
#endif


        });

        return true;
    }
}
