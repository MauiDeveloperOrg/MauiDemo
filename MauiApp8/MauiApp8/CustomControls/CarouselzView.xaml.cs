using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MauiApp8.CustomControls;

public partial class CarouselzView : TemplatedView
{
	public CarouselzView()
	{
		InitializeComponent();
        BackgroundColor = Colors.Transparent;
        WidthRequest = 1000d;
        HeightRequest = 350d;
        Padding = new(0d);
    }

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
                                                          propertyName: nameof(ItemsSource),
                                                          returnType: typeof(IEnumerable),
                                                          declaringType: typeof(CarouselzView),
                                                          defaultValue: default,
                                                          defaultBindingMode: BindingMode.TwoWay,
                                                          propertyChanged: OnItemsSourcePropertyChanged);

    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
                                                                   propertyName: nameof(ItemTemplate),
                                                                   returnType: typeof(DataTemplate),
                                                                   declaringType: typeof(CarouselzView),
                                                                   defaultValue: default,
                                                                   defaultBindingMode: BindingMode.TwoWay,
                                                                   propertyChanged: OnItemTemplatePropertyChanged);

    public static readonly BindableProperty ItemTemplateSelectorProperty = BindableProperty.Create(
                                                                           propertyName: nameof(ItemTemplateSelector),
                                                                           returnType: typeof(DataTemplateSelector),
                                                                           declaringType: typeof(CarouselzView),
                                                                           defaultValue: default,
                                                                           defaultBindingMode: BindingMode.TwoWay,
                                                                           propertyChanged: OnItemTemplateSelectorPropertyChanged);

    public static readonly BindableProperty EmptyViewProperty = BindableProperty.Create(
                                                                propertyName: nameof(EmptyView),
                                                                returnType: typeof(object),
                                                                declaringType: typeof(CarouselzView),
                                                                defaultValue: default,
                                                                defaultBindingMode: BindingMode.TwoWay,
                                                                propertyChanged: OnEmptyViewPropertyChanged);

    public static readonly BindableProperty EmptyViewTemplateProperty = BindableProperty.Create(
                                                                        propertyName: nameof(EmptyViewTemplate),
                                                                        returnType: typeof(DataTemplate),
                                                                        declaringType: typeof(CarouselzView),
                                                                        defaultValue: default,
                                                                        defaultBindingMode: BindingMode.TwoWay,
                                                                        propertyChanged: OnEmptyViewPropertyChanged);

    public static readonly BindableProperty VisibleViewsProperty = BindableProperty.Create(
                                                                   propertyName: nameof(VisibleViews),
                                                                   returnType: typeof(ObservableCollection<View>),
                                                                   declaringType: typeof(CarouselzView),
                                                                   defaultValue: new ObservableCollection<View>(),
                                                                   defaultBindingMode: BindingMode.OneWay);

    public static readonly BindableProperty LoopProperty = BindableProperty.Create(
                                                           propertyName: nameof(Loop),
                                                           returnType: typeof(bool),
                                                           declaringType: typeof(CarouselzView),
                                                           defaultValue: false,
                                                           defaultBindingMode: BindingMode.TwoWay,
                                                           propertyChanged: OnLoopPropertyChanged);

    public static readonly BindableProperty IntervalProperty = BindableProperty.Create(
                                                               propertyName: nameof(Interval),
                                                               returnType: typeof(double),
                                                               declaringType: typeof(CarouselzView),
                                                               defaultValue: 400d,
                                                               defaultBindingMode: BindingMode.TwoWay,
                                                               propertyChanged: OnIntervalPropertyChanged);

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public DataTemplate ItemTemplate
    {
        get => (DataTemplate)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public DataTemplateSelector ItemTemplateSelector
    {
        get => (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
        set => SetValue(ItemTemplateSelectorProperty, value);
    }

    public object EmptyView
    {
        get => GetValue(EmptyViewProperty);
        set => SetValue(EmptyViewProperty, value);
    }

    public DataTemplate EmptyViewTemplate
    {
        get => (DataTemplate)GetValue(EmptyViewTemplateProperty);
        set => SetValue(EmptyViewTemplateProperty, value);
    }

    public ObservableCollection<View> VisibleViews => (ObservableCollection<View>)GetValue(VisibleViewsProperty);

    public bool Loop
    {
        get => (bool)GetValue(LoopProperty);
        set => SetValue(LoopProperty, value);
    }

    public double Interval
    {
        get => (double)GetValue(IntervalProperty);
        set => SetValue(IntervalProperty, value);
    }

    private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CarouselzView view)
            return;

        if (oldValue is INotifyCollectionChanged oldCollections)
            oldCollections.CollectionChanged -= view.ItemsSourceCollectionChanged;

        if (newValue is INotifyCollectionChanged newCollections)
            newCollections.CollectionChanged += view.ItemsSourceCollectionChanged;

        if (view.PART_Container.IsLoaded)
            view.CreateChildren();
    }

    private static void OnItemTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CarouselzView view)
            return;
    }

    private static void OnItemTemplateSelectorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CarouselzView view)
            return;
    }

    private static void OnEmptyViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CarouselzView view)
            return;

        view._currentEmptyView = view.CreateEmptyView(view.EmptyView, view.EmptyViewTemplate);
    }

    private static void OnIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CarouselzView view)
            return;

 
    }

    private static void OnLoopPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CarouselzView view)
            return;

      
    }

}

public partial class CarouselzView
{
    readonly double _scal = 0.3;
    readonly Dictionary<int, View> _mapViews = new();
    readonly Dictionary<int, Rect> _mapDockRects = new();
    readonly Dictionary<int, View> _mapRealView = new();

    AbsoluteLayout PART_Container = default!;
    Rect _showPanel;
    Rect _dockPanel;

    View? _currentEmptyView;
    int _currentIndex = 0;

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var templateObject = GetTemplateChild(nameof(PART_Container));
        if (templateObject is AbsoluteLayout container)
            PART_Container = container;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_Container));

        PART_Container.Loaded += PART_Container_Loaded;
        PART_Container.SizeChanged += PART_Container_SizeChanged;
    }

    void PART_Container_Loaded(object? sender, EventArgs e)
    {
        CreateRect();
        CreateChildren();
    }

    void PART_Container_SizeChanged(object? sender, EventArgs e)
    {
        if (!PART_Container.IsLoaded)
            return;

        CreateRect();
        CreateChildren();
    }

    void ItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {


    }
}

public partial class CarouselzView
{
    void CreateRect()
    {
        var width = PART_Container.Width;
        var height = PART_Container.Height;

        if (double.IsNaN(width) || double.IsNaN(height) || width <= 0 || height <= 0)
            return;

        var rectHeight = height * _scal;
        _showPanel = new Rect(0, 0, width, height - rectHeight);
        _dockPanel = new Rect(0, height - rectHeight, width, rectHeight);
    }

    void CreateChildren()
    {
        _mapViews.Clear();
        _mapDockRects.Clear();
        _mapRealView.Clear();
        VisibleViews.Clear();
        PART_Container.Clear();
        if (!ItemsSource?.GetEnumerator().MoveNext() ?? true)
        {
            PART_Container.Add(CreateEmptyView(EmptyView, EmptyViewTemplate));
            return;
        }



    }

    View CreateEmptyView(object? emptyView, DataTemplate? dataTemplate)
    {
        if (dataTemplate is not null)
        {
            var view = (View)dataTemplate.CreateContent();
            view.BindingContext = this.BindingContext;
            return view;
        }

        if (emptyView is View emptyLayout)
            return emptyLayout;

        return new Label { Text = emptyView?.ToString(), HorizontalTextAlignment = TextAlignment.Center };
    }

    View CreateItemView(object item) => CreateItemView(item, ItemTemplate ?? ItemTemplateSelector?.SelectTemplate(item, this));

    View CreateItemView(object item, DataTemplate? dataTemplate)
    {
        if (dataTemplate is null)
            return new Label { Text = item?.ToString(), HorizontalTextAlignment = TextAlignment.Center };
        else
        {
            var view = (View)dataTemplate.CreateContent();
            view.BindingContext = item;
            return view;
        }
    }
}