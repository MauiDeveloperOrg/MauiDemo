namespace MauiApp8.CustomControls;

public partial class ProgressButton : Frame
{
    public ProgressButton()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
            propertyName: nameof(Text),
            returnType: typeof(string),
            declaringType: typeof(ProgressButton),
            defaultValue: "",
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: TextPropertyChanged);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty IsInProgressingProperty = BindableProperty.Create(
        propertyName: nameof(IsInProgressing),
        returnType: typeof(bool),
        declaringType: typeof(ProgressButton),
        defaultValue: false,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: IsInProgressingPropertyChanged);

    public bool IsInProgressing
    {
        get => (bool)GetValue(IsInProgressingProperty);
        set => SetValue(IsInProgressingProperty, value);
    }

    private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {

    }

    private static void IsInProgressingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
         
    }

}