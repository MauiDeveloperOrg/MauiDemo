namespace MauiApp8.CustomControls;

public partial class Expander : TemplatedView
{
    public Expander()
    {
        InitializeComponent();
        WidthRequest = 300;
    }

    public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
                                                                  propertyName: nameof(BorderColor),
                                                                  returnType: typeof(Color),
                                                                  declaringType: typeof(Expander),
                                                                  defaultValue: Colors.Transparent,
                                                                  defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create(
                                                                      propertyName: nameof(BorderThickness),
                                                                      returnType: typeof(double),
                                                                      declaringType: typeof(Expander),
                                                                      defaultValue: 0d,
                                                                      defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty HeaderProperty = BindableProperty.Create(
                                                                  propertyName: nameof(Header),
                                                                  returnType: typeof(string),
                                                                  declaringType: typeof(Expander),
                                                                  defaultValue: default,
                                                                  defaultBindingMode: BindingMode.TwoWay);





    public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(
                                                                 propertyName: nameof(IsExpanded),
                                                                 returnType: typeof(bool),
                                                                 declaringType: typeof(Expander),
                                                                 defaultValue: true,
                                                                 defaultBindingMode: BindingMode.TwoWay,
                                                                 propertyChanged: OnIsExpandedChanged);

    public static readonly BindableProperty ContentProperty = BindableProperty.Create(
                                                              propertyName: nameof(Content),
                                                              returnType: typeof(View),
                                                              declaringType: typeof(Expander),
                                                              defaultValue: default,
                                                              defaultBindingMode: BindingMode.TwoWay);



    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    public double BorderThickness
    {
        get => (double)GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }

    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    public View Content
    {
        get => (View)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    private static void OnIsExpandedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not Expander expander)
            return;

        var partContent = expander._partContent;
        if (partContent is null)
            return;

        if (!bool.TryParse(newValue?.ToString(), out var isExpanded))
            return;

        //var animation = new Animation();
        if (isExpanded)
        {
            partContent.ScaleYTo(1);
            //partContent.IsVisible = true;
            //var moveAnimation = new Animation(h =>
            //{
            //    partContent.HeightRequest = h;
            //}, 0, expander._contentHeight);
            //animation.Insert(0, 1, moveAnimation);
        }
        else
        {
            //partContent.LayoutTo(Rect.Zero);
            partContent.ScaleYTo(0);
         
            //expander._contentBound = partContent.Frame;
            ////partContent.LayoutTo(Rect.Zero);
            //expander._contentHeight = partContent.Height;
            //var moveAnimation = new Animation(h =>
            //{
            //    partContent.HeightRequest = h;

            //}, partContent.Height, 0, finished: () =>
            //{
            //    partContent.IsVisible = false;
            //});
            //animation.Insert(0, 1, moveAnimation);
        }

        //animation.Commit(partContent, "MoveAnimation", 16, 1000, Easing.SinInOut, finished: (x, b) =>
        //{
        //    partContent.CancelAnimations();
        //    animation.Dispose();
        //});
    }


}

public partial class Expander
{


    Command<object>? _tappedCommand = default;
    public Command<object>? TappedCommand => _tappedCommand ??= new(t =>
    {
        IsExpanded = !IsExpanded;
    });


}



public partial class Expander
{
    const string _PART_ContentName = "PART_ContentDock";

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var dock = GetTemplateChild(_PART_ContentName);
        if (dock is not Frame frame)
            throw new ArgumentNullException(_PART_ContentName);

        _partContent = frame;
    }

    Frame? _partContent = default;
    Rect _contentBound = Rect.Zero;
    double _contentHeight = 0;
}
