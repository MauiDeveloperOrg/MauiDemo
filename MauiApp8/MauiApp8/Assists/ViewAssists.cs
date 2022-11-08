namespace MauiApp8.Assists;

public static class ViewAssists
{
    public static readonly BindableProperty TagProperty = BindableProperty.CreateAttached(
                                                          propertyName: "Tag",
                                                          returnType: typeof(object),
                                                          declaringType:typeof(ViewAssists),
                                                          defaultValue:default);

    public static void SetTag(BindableObject view, object value) => view.SetValue(TagProperty, value);
    public static object GetTag(BindableObject view) => view.GetValue(TagProperty); 
}
