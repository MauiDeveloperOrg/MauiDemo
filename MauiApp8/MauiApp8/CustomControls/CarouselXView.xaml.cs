using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace MauiApp8.CustomControls;

internal enum DockAlignment
{
    Start = 0,
    Center = 1,
    End = 2,
}

public partial class CarouselXView : TemplatedView
{
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
                                                              propertyName: nameof(ItemsSource),
                                                              returnType: typeof(IEnumerable),
                                                              declaringType: typeof(CarouselXView),
                                                              defaultValue: default,
                                                              defaultBindingMode: BindingMode.TwoWay,
                                                              propertyChanged: OnItemsSourcePropertyChanged);

    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
                                                                   propertyName: nameof(ItemTemplate),
                                                                   returnType: typeof(DataTemplate),
                                                                   declaringType: typeof(CarouselXView),
                                                                   defaultValue: default,
                                                                   defaultBindingMode: BindingMode.TwoWay,
                                                                   propertyChanged: OnItemTemplatePropertyChanged);

    public static readonly BindableProperty ItemTemplateSelectorProperty = BindableProperty.Create(
                                                                           propertyName: nameof(ItemTemplateSelector),
                                                                           returnType: typeof(DataTemplateSelector),
                                                                           declaringType: typeof(CarouselXView),
                                                                           defaultValue: default,
                                                                           defaultBindingMode: BindingMode.TwoWay,
                                                                           propertyChanged: OnItemTemplateSelectorPropertyChanged);

    public static readonly BindableProperty EmptyViewProperty = BindableProperty.Create(
                                                                propertyName: nameof(EmptyView),
                                                                returnType: typeof(object),
                                                                declaringType: typeof(CarouselXView),
                                                                defaultValue: default,
                                                                defaultBindingMode: BindingMode.TwoWay,
                                                                propertyChanged: OnEmptyViewPropertyChanged);

    public static readonly BindableProperty EmptyViewTemplateProperty = BindableProperty.Create(
                                                                        propertyName: nameof(EmptyViewTemplate),
                                                                        returnType: typeof(DataTemplate),
                                                                        declaringType: typeof(CarouselXView),
                                                                        defaultValue: default,
                                                                        defaultBindingMode: BindingMode.TwoWay,
                                                                        propertyChanged: OnEmptyViewPropertyChanged);

    public static readonly BindableProperty VisibleViewsProperty = BindableProperty.Create(
                                                                   propertyName: nameof(VisibleViews),
                                                                   returnType: typeof(ObservableCollection<View>),
                                                                   declaringType: typeof(CarouselXView),
                                                                   defaultValue: new ObservableCollection<View>(),
                                                                   defaultBindingMode: BindingMode.OneWay);

    public static readonly BindableProperty LoopProperty = BindableProperty.Create(
                                                           propertyName: nameof(Loop),
                                                           returnType: typeof(bool),
                                                           declaringType: typeof(CarouselXView),
                                                           defaultValue: false,
                                                           defaultBindingMode: BindingMode.TwoWay);

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

    public ObservableCollection<View> VisibleViews
    {
        get => (ObservableCollection<View>)GetValue(VisibleViewsProperty);
    }

    public bool Loop
    {
        get => (bool)GetValue(LoopProperty);
        set => SetValue(LoopProperty, value);
    }

    private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CarouselXView view)
            return;

        if (oldValue is INotifyCollectionChanged oldCollections)
            oldCollections.CollectionChanged -= view.ItemsSourceCollectionChanged;

        if (newValue is INotifyCollectionChanged newCollections)
            newCollections.CollectionChanged += view.ItemsSourceCollectionChanged;

        view.CreateChildren();
    }

    private static void OnItemTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CarouselXView view)
            return;
    }

    private static void OnItemTemplateSelectorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CarouselXView view)
            return;
    }

    private static void OnEmptyViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CarouselXView view)
            return;

        view._currentEmptyView = view.CreateEmptyView(view.EmptyView, view.EmptyViewTemplate);
    }
}

public partial class CarouselXView 
{
    public CarouselXView()
    {
        InitializeComponent();
        BackgroundColor = Colors.Transparent;
        WidthRequest = 1000d;
        HeightRequest = 300d;
        var templateObject = GetTemplateChild(nameof(PART_Container));
        if (templateObject is AbsoluteLayout container)
            PART_Container = container;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_Container));

        PART_Container.Loaded += PART_Container_Loaded;
        PART_Container.SizeChanged += PART_Container_SizeChanged;
    }

    readonly AbsoluteLayout PART_Container = default!;
    readonly Dictionary<int, View> _mapViews = new();
    readonly Dictionary<DockAlignment, View> _mapDockViews = new();
    readonly Dictionary<DockAlignment, Rect> _mapDockRects = new();

    View? _currentEmptyView;
    readonly double _scalX = 0.3;
    readonly double _scalY = 0.2;

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
    }

    void PART_Container_Loaded(object? sender, EventArgs e)
    {
        CreateRect();
        CreateChildren();
    }

    void PART_Container_SizeChanged(object? sender, EventArgs e)
    {
        CreateRect();
    }

    void ItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {

    }

    void CreateChildren()
    {
        PART_Container.Clear();
        _mapViews.Clear();
        _mapDockViews.Clear();

        if (ItemsSource?.GetEnumerator().MoveNext() ?? false)
        {
            PART_Container.Add(CreateEmptyView(EmptyView, EmptyViewTemplate));
            return;
        }

        int index = 0;
        foreach (var item in ItemsSource!)
        {
            var view = CreateItemView(item);
            PART_Container.Add(view);
            _mapViews[index] = view;

            index++;

            if (!_mapDockViews.TryGetValue(DockAlignment.Start, out _))
            {
                _mapDockViews[DockAlignment.Start] = view;
                if (_mapDockRects.TryGetValue(DockAlignment.Start, out var dock))
                    AbsoluteLayout.SetLayoutBounds(view, dock);
                break;
            }

            if (!_mapDockViews.TryGetValue(DockAlignment.Center, out _))
            {
                _mapDockViews[DockAlignment.Center] = view;
                if (_mapDockRects.TryGetValue(DockAlignment.Center, out var dock))
                    AbsoluteLayout.SetLayoutBounds(view, dock);
                break;
            }

            if (_mapDockViews.TryGetValue(DockAlignment.End, out _))
            {
                _mapDockViews[DockAlignment.End] = view;
                if (_mapDockRects.TryGetValue(DockAlignment.End, out var dock))
                    AbsoluteLayout.SetLayoutBounds(view, dock);
                break;
            }
        }
    }

    void CreateRect()
    {
        var width = PART_Container.Width - Padding.HorizontalThickness;
        var height = PART_Container.Height - Padding.VerticalThickness;

        if (double.IsNaN(width) || double.IsNaN(height) || width <= 0 || height <= 0)
            return;

        double offsetX = width * _scalX;
        double offsetY = height * _scalY;

        var viewWidth = width - 2 * offsetX;
        var viewHeight = height;

        double left = offsetX;
        double top = 0;

        var rect = new Rect(left,top, viewWidth, viewHeight);
        _mapDockRects[DockAlignment.Center] = rect;

        viewHeight = height - offsetY;
        rect = new Rect(0, offsetY, viewWidth, viewHeight);
        _mapDockRects[DockAlignment.Start] = rect;

        rect = new Rect(width - viewWidth, offsetY, viewWidth, viewHeight);
        _mapDockRects[DockAlignment.End] = rect;
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


