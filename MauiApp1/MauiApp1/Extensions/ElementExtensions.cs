﻿#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UIView;
using BasePlatformType = ObjCRuntime.INativeObject;
using PlatformWindow = UIKit.UIWindow;
using PlatformApplication = UIKit.IUIApplicationDelegate;
#elif ANDROID || MONOANDROID
using PlatformView = Android.Views.View;
using BasePlatformType = Android.Content.Context;
using PlatformWindow = Android.App.Activity;
using PlatformApplication = Android.App.Application;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
using BasePlatformType = WinRT.IWinRTObject;
using PlatformWindow = Microsoft.UI.Xaml.Window;
using PlatformApplication = Microsoft.UI.Xaml.Application;
#elif TIZEN
using PlatformView = ElmSharp.EvasObject;
using BasePlatformType = System.Object;
using PlatformWindow = ElmSharp.Window;
using PlatformApplication = Tizen.Applications.CoreUIApplication;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0 && !IOS && !ANDROID && !MONOANDROID && !TIZEN)
using BasePlatformType = System.Object;
using PlatformApplication = System.Object;
using PlatformView = System.Object;
using PlatformWindow = System.Object;
#endif

using IPlatformViewHandler = Microsoft.Maui.IViewHandler;

namespace MauiApp1.Extensions;
public static class ElementExtensions
{
    public static void SetHandler(this BasePlatformType nativeElement, IElement element, IMauiContext context)
    {
        _ = nativeElement ?? throw new ArgumentNullException(nameof(nativeElement));
        _ = element ?? throw new ArgumentNullException(nameof(element));
        _ = context ?? throw new ArgumentNullException(nameof(context));

        var handler = element.Handler;
        if (handler?.MauiContext != null && handler.MauiContext != context)
            handler = null;

        if (handler == null)
            handler = context.Handlers.GetHandler(element.GetType());

        if (handler == null)
            throw new Exception($"Handler not found for window {element}.");

        handler.SetMauiContext(context);

        element.Handler = handler;

        if (handler.VirtualView != element)
            handler.SetVirtualView(element);
    }

    public static PlatformView ToPlatform(this IElement view)
    {
        if (view is IReplaceableView replaceableView && replaceableView.ReplacedView != view)
            return replaceableView.ReplacedView.ToPlatform();

        _ = view.Handler ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set on parent.");

        if (view.Handler is IPlatformViewHandler viewHandler)
        {
            if (viewHandler.ContainerView is PlatformView containerView)
                return containerView;

            if (viewHandler.PlatformView is PlatformView platformView)
                return platformView;
        }

        return (view.Handler?.PlatformView as PlatformView) ?? throw new InvalidOperationException($"Unable to convert {view} to {typeof(PlatformView)}");

    }

    public static IEnumerable<Element> GetParentsPath(this Element self)
    {
        Element current = self;

        while (!current.RealParent.IsApplicationOrNull())
        {
            current = current.RealParent;
            yield return current;
        }
    }

    public static Element? FindParentWith(this Element element, Func<Element?, bool>? withMatch, bool includeThis = false)
    {

        if (includeThis && withMatch?.Invoke(element) == true)
            return element;

        foreach (var parent in element.GetParentsPath())
        {
            if (withMatch?.Invoke(parent) == true)
                return parent;
        }

        return default;
    }

    public static T? FindParentOfType<T>(this IElement element, bool includeThis = false) where T : IElement
    {
        if (includeThis && element is T view)
            return view;

        foreach (var parent in element.GetParentsPath())
        {
            if (parent is T parentView)
                return parentView;
        }

        return default;
    }

    public static IEnumerable<IElement?> GetParentsPath(this IElement self)
    {
        IElement? current = self;

        while (current != null && current is not IApplication)
        {
            current = current.Parent;
            yield return current;
        }
    }

}
