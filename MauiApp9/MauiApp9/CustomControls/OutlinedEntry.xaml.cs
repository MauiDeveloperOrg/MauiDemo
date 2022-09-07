namespace MauiApp9.CustomControls;

public partial class OutlinedEntry : Grid
{
	public OutlinedEntry()
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

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
      propertyName: nameof(Placeholder),
      returnType: typeof(string),
      declaringType: typeof(ProgressButton),
      defaultValue: "",
      defaultBindingMode: BindingMode.TwoWay,
      propertyChanged: PlaceholderPropertyChanged);


    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }


    private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
         
    }

    private static void PlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
         
    }

    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        lblPlaceholder.FontSize = 11;
        lblPlaceholder.TranslateTo(0, -26, 80, Easing.Linear);
        lblPlaceholder.BackgroundColor = Colors.White;
        lblPlaceholder.ZIndex = 1;
        faeBorder.ZIndex = 0;
    }

    private void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(Text))
        {

        }
        else
        {
            lblPlaceholder.FontSize = 15;
            lblPlaceholder.TranslateTo(0, 0, 80, Easing.Linear);
            lblPlaceholder.BackgroundColor = Colors.Transparent;
            lblPlaceholder.ZIndex = 0;
            faeBorder.ZIndex = 1;
        }

       
    }
}