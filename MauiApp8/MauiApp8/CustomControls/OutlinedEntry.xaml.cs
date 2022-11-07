namespace MauiApp8.CustomControls;

public partial class OutlinedEntry : TemplatedView
{
	public OutlinedEntry()
	{
		InitializeComponent();
        var templateObject = GetTemplateChild(nameof(PART_lblPlaceholder));
        if (templateObject is Label label)
            PART_lblPlaceholder = label;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_lblPlaceholder));

        templateObject = GetTemplateChild(nameof(PART_faeBorder));
        if (templateObject is Frame frame)
            PART_faeBorder = frame;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_faeBorder));
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
                                                           propertyName: nameof(Text),
                                                           returnType: typeof(string),
                                                           declaringType: typeof(OutlinedEntry),
                                                           defaultValue: "",
                                                           defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
                                                                  propertyName: nameof(Placeholder),
                                                                  returnType: typeof(string),
                                                                  declaringType: typeof(OutlinedEntry),
                                                                  defaultValue: "",
                                                                  defaultBindingMode: BindingMode.TwoWay);

    readonly Label PART_lblPlaceholder = default!;
    readonly Frame PART_faeBorder = default!;

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        PART_lblPlaceholder.FontSize = 11;
        PART_lblPlaceholder.TranslateTo(0, -26, 80, Easing.Linear);
        PART_lblPlaceholder.BackgroundColor = Colors.White;
        PART_lblPlaceholder.ZIndex = 1;
        PART_faeBorder.ZIndex = 0;
    }

    private void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(Text))
        {

        }
        else
        {
            PART_lblPlaceholder.FontSize = 15;
            PART_lblPlaceholder.TranslateTo(0, 0, 80, Easing.Linear);
            PART_lblPlaceholder.BackgroundColor = Colors.Transparent;
            PART_lblPlaceholder.ZIndex = 0;
            PART_faeBorder.ZIndex = 1;
        }
    }
}