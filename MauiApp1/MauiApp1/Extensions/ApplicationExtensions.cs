using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Extensions;
public static class ApplicationExtensions
{
    public static bool IsApplicationOrNull(this object element) => element == null || element is IApplication;

    public static bool IsApplicationOrWindowOrNull(this object element) => element == null || element is IApplication || element is IWindow;
}