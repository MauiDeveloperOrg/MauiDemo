using MauiApp8.Assists; 
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
        HeightRequest = 450d;
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
    readonly double _offset = 20d;
    readonly double _maxHeight = 80d;
    readonly Dictionary<int, View> _mapViews = new();
    readonly Dictionary<int, Rect> _mapDockRects = new();
    readonly Dictionary<int, int> _mapRealViewInDocks = new();

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
        if (rectHeight > _maxHeight)
            rectHeight = _maxHeight;
        _showPanel = new Rect(0, 0, width, height - rectHeight - _offset);
        _dockPanel = new Rect(0, height - rectHeight, width, rectHeight);
    }

    void CalculateRect(int count)
    {
        if (count <= 0)
            return;
        double avgWidth = _dockPanel.Width / count;
        if (avgWidth <= 0)
            return;
        double left = 0;
        double top = _dockPanel.Top;
        double height = _dockPanel.Height;

        for (int i = 0; i < count; i++)
        {
            left = i * avgWidth;
            var rect = new Rect(left, top, avgWidth, height);
            _mapDockRects[i] = rect;
        }
    }

    void CreateChildren()
    {
        _mapViews.Clear();
        _mapDockRects.Clear();
        _mapRealViewInDocks.Clear();
        VisibleViews.Clear();
        PART_Container.Clear();
        if (!ItemsSource?.GetEnumerator().MoveNext() ?? true)
        {
            PART_Container.Add(CreateEmptyView(EmptyView, EmptyViewTemplate));
            return;
        }

        int index = 0;
        foreach (var item in ItemsSource!)
        {
            var view = CreateItemView(item);
            ViewAssists.SetTag(view, index);
            VisibleViews.Add(view);
            PART_Container.Add(view);
            _mapViews[index] = view;
            ++index;

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            view.GestureRecognizers.Add(tapGestureRecognizer);
        }

        if (_mapViews.TryGetValue(_currentIndex, out var showView))
            AbsoluteLayout.SetLayoutBounds(showView, _showPanel);

        CalculateRect(index - 1);
        MoveChildren();
    }

    void MoveChildren()
    {
        int realIndex = 0;
        for (int i = 0; i < VisibleViews.Count; i++)
        {
            if (i == _currentIndex)
                continue;

            if (!_mapDockRects.TryGetValue(realIndex, out var rect))
                continue;

            AbsoluteLayout.SetLayoutBounds(VisibleViews[i], rect);
            _mapRealViewInDocks[i] = realIndex;

            ++realIndex;
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

    private void TapGestureRecognizer_Tapped(object? sender, EventArgs e)
    {
        if (sender is not View view)
            return;

        CommitAnimation(view);
    }

}

public partial class CarouselzView
{
    void CommitAnimation(View view)
    {
        if (ViewAssists.GetTag(view) is not int index)
            return;

        if (_currentIndex == index)
            return;

        int realIndex = 0;
        Dictionary<int, View> mapDockViews = new();
        for (int i = 0; i < VisibleViews.Count; i++)
        {
            if (i == index)
                continue;

            mapDockViews[realIndex] = VisibleViews[i];
            ++realIndex;
        }

        var animation = new Animation();

        {
            view.ZIndex = 1;
            var rect = AbsoluteLayout.GetLayoutBounds(view);
            var move2Rect = _showPanel;

            var newRect = new Rect((move2Rect.Width - rect.Width) / 2, (move2Rect.Height - rect.Height) / 2, rect.Width, rect.Height);
            double scalw = move2Rect.Width / rect.Width;
            double scalh = move2Rect.Height / rect.Height;
            double scalx = newRect.X - rect.X;
            double scaly = newRect.Y - rect.Y;

            var moveAnimation = new Animation(s =>
            {
                double newX = scalx * s + rect.X;
                double newY = scaly * s + rect.Y;

                var pRect = new Rect(newX, newY, rect.Width, rect.Height);
                AbsoluteLayout.SetLayoutBounds(view, pRect);
            }, 0, 1);
            animation.Insert(0, 0.5, moveAnimation);

            var transAnimation = new Animation(s =>
            {
                double newWidth = scalw * s * rect.Width;
                double newHeight = scalh * s * rect.Height;
                double newX = (move2Rect.Width - newWidth) / 2;
                double newY = (move2Rect.Height - newHeight) / 2;

                var pRect = new Rect(newX, newY, newWidth, newHeight);
                AbsoluteLayout.SetLayoutBounds(view, pRect);
            }, 0, 1);
            animation.Insert(0.5, 1, transAnimation);
        }

        animation.Commit(this, $"MoveAnimation{Guid.NewGuid()}", length: 1000, easing: Easing.SinInOut, finished: (d, b) =>
        {
            _currentIndex = index;
            view.ZIndex = 0;
            animation.Dispose();
        });
    }





}

