using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Windows.Input; 

namespace MauiApp8.CustomControls;

internal enum DockAlignment
{
    Default = 0,
    Start = 1,
    Center = 2,
    End = 3,
}

public partial class CarouselxView : TemplatedView
{
    public CarouselxView()
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
                                                              declaringType: typeof(CarouselxView),
                                                              defaultValue: default,
                                                              defaultBindingMode: BindingMode.TwoWay,
                                                              propertyChanged: OnItemsSourcePropertyChanged);

    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
                                                                   propertyName: nameof(ItemTemplate),
                                                                   returnType: typeof(DataTemplate),
                                                                   declaringType: typeof(CarouselxView),
                                                                   defaultValue: default,
                                                                   defaultBindingMode: BindingMode.TwoWay,
                                                                   propertyChanged: OnItemTemplatePropertyChanged);

    public static readonly BindableProperty ItemTemplateSelectorProperty = BindableProperty.Create(
                                                                           propertyName: nameof(ItemTemplateSelector),
                                                                           returnType: typeof(DataTemplateSelector),
                                                                           declaringType: typeof(CarouselxView),
                                                                           defaultValue: default,
                                                                           defaultBindingMode: BindingMode.TwoWay,
                                                                           propertyChanged: OnItemTemplateSelectorPropertyChanged);

    public static readonly BindableProperty EmptyViewProperty = BindableProperty.Create(
                                                                propertyName: nameof(EmptyView),
                                                                returnType: typeof(object),
                                                                declaringType: typeof(CarouselxView),
                                                                defaultValue: default,
                                                                defaultBindingMode: BindingMode.TwoWay,
                                                                propertyChanged: OnEmptyViewPropertyChanged);

    public static readonly BindableProperty EmptyViewTemplateProperty = BindableProperty.Create(
                                                                        propertyName: nameof(EmptyViewTemplate),
                                                                        returnType: typeof(DataTemplate),
                                                                        declaringType: typeof(CarouselxView),
                                                                        defaultValue: default,
                                                                        defaultBindingMode: BindingMode.TwoWay,
                                                                        propertyChanged: OnEmptyViewPropertyChanged);

    public static readonly BindableProperty VisibleViewsProperty = BindableProperty.Create(
                                                                   propertyName: nameof(VisibleViews),
                                                                   returnType: typeof(ObservableCollection<View>),
                                                                   declaringType: typeof(CarouselxView),
                                                                   defaultValue: new ObservableCollection<View>(),
                                                                   defaultBindingMode: BindingMode.OneWay);

    public static readonly BindableProperty LoopProperty = BindableProperty.Create(
                                                           propertyName: nameof(Loop),
                                                           returnType: typeof(bool),
                                                           declaringType: typeof(CarouselxView),
                                                           defaultValue: false,
                                                           defaultBindingMode: BindingMode.TwoWay,
                                                           propertyChanged: OnLoopPropertyChanged);

    public static readonly BindableProperty IntervalProperty = BindableProperty.Create(
                                                               propertyName: nameof(Interval),
                                                               returnType: typeof(double),
                                                               declaringType: typeof(CarouselxView),
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
        if (bindable is not CarouselxView view)
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
        if (bindable is not CarouselxView view)
            return;
    }

    private static void OnItemTemplateSelectorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CarouselxView view)
            return;
    }

    private static void OnEmptyViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CarouselxView view)
            return;

        view._currentEmptyView = view.CreateEmptyView(view.EmptyView, view.EmptyViewTemplate);
    }

    private static async void OnIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CarouselxView view)
            return;

        view.StopLoop();
        await view.StartLoop(); 
    }

    private static async void OnLoopPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CarouselxView view)
            return;

        bool.TryParse(newValue?.ToString(), out var vRet);
        view.StopLoop();
        if (vRet)
            await view.StartLoop();
    }
}

public partial class CarouselxView
{
    readonly Dictionary<int, View> _mapViews = new();
    readonly Dictionary<DockAlignment, View> _mapDockViews = new();
    readonly Dictionary<DockAlignment, Rect> _mapDockRects = new();

    AbsoluteLayout PART_Container = default!;
    View? _currentEmptyView;
    readonly double _scalX = 0.3;
    readonly double _scalY = 0.13;

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
        MoveChildern();
    }

    void ItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                break;
            case NotifyCollectionChangedAction.Remove:
                break;
            case NotifyCollectionChangedAction.Replace:
                break;
            case NotifyCollectionChangedAction.Move:
                break;
            case NotifyCollectionChangedAction.Reset:
                break;
            default:
                break;
        }
    }

    void CreateRect()
    {
        var width = PART_Container.Width;
        var height = PART_Container.Height;

        if (double.IsNaN(width) || double.IsNaN(height) || width <= 0 || height <= 0)
            return;

        double offsetX = width * _scalX / 2d;
        double offsetY = height * _scalY / 2d;

        var viewWidth = width - 2 * offsetX;
        var viewHeight = height;

        double left = offsetX;
        double top = 0;

        var rect = new Rect(left, top, viewWidth, viewHeight);
        _mapDockRects[DockAlignment.Center] = rect;

        viewHeight = height - offsetY * 2;
        rect = new Rect(0, offsetY, viewWidth, viewHeight);
        _mapDockRects[DockAlignment.Start] = rect;

        rect = new Rect(width - viewWidth, offsetY, viewWidth, viewHeight);
        _mapDockRects[DockAlignment.End] = rect;

        rect = new Rect(left, offsetY, viewWidth, viewHeight);
        _mapDockRects[DockAlignment.Default] = rect;
    }

    async void CreateChildren()
    {
        StopLoop();
        VisibleViews.Clear();
        PART_Container.Clear();
        _mapViews.Clear();
        _mapDockViews.Clear();

        if (!ItemsSource?.GetEnumerator().MoveNext() ?? true)
        {
            PART_Container.Add(CreateEmptyView(EmptyView, EmptyViewTemplate));
            return;
        }

        int index = 0;
        foreach (var item in ItemsSource!)
        {
            var view = CreateItemView(item);
            VisibleViews.Add(view);
            PART_Container.Add(view);
            _mapViews[index] = view;
            index++;

            if (!_mapDockViews.TryGetValue(DockAlignment.Start, out _))
            {
                view.ZIndex = 1;
                _mapDockViews[DockAlignment.Start] = view;
                if (_mapDockRects.TryGetValue(DockAlignment.Start, out var dock))
                    AbsoluteLayout.SetLayoutBounds(view, dock);
                continue;
            }

            if (!_mapDockViews.TryGetValue(DockAlignment.Center, out _))
            {
                view.ZIndex = 2;
                _mapDockViews[DockAlignment.Center] = view;
                if (_mapDockRects.TryGetValue(DockAlignment.Center, out var dock))
                    AbsoluteLayout.SetLayoutBounds(view, dock);
                continue;
            }

            if (!_mapDockViews.TryGetValue(DockAlignment.End, out _))
            {
                view.ZIndex = 1;
                _mapDockViews[DockAlignment.End] = view;
                if (_mapDockRects.TryGetValue(DockAlignment.End, out var dock))
                    AbsoluteLayout.SetLayoutBounds(view, dock);
                continue;
            }

            {
                view.ZIndex = 0;
                view.IsVisible = false;
                if (_mapDockRects.TryGetValue(DockAlignment.Default, out var dock))
                    AbsoluteLayout.SetLayoutBounds(view, dock);
            }
        }

       await StartLoop();
    }

    void MoveChildern()
    {
        if (_mapDockViews.TryGetValue(DockAlignment.Start, out var view1))
        {
            if (_mapDockRects.TryGetValue(DockAlignment.Start, out var dock))
                AbsoluteLayout.SetLayoutBounds(view1, dock);
        }

        if (_mapDockViews.TryGetValue(DockAlignment.Center, out var view2))
        {
            if (_mapDockRects.TryGetValue(DockAlignment.Center, out var dock))
                AbsoluteLayout.SetLayoutBounds(view2, dock);
        }

        if (_mapDockViews.TryGetValue(DockAlignment.End, out var view3))
        {
            if (_mapDockRects.TryGetValue(DockAlignment.End, out var dock))
                AbsoluteLayout.SetLayoutBounds(view3, dock);
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

    async Task<bool> StartLoop()
    {
        if (!Loop)
            return false;

        if (!IsLoaded)
            return false;

        if (_mapDockViews.Count <= 0)
            return false;

        if (Interval <= 0)
            return false;

        _timer = new(TimeSpan.FromMilliseconds(Interval));

        for (; ; )
        {
            var result = await _timer.WaitForNextTickAsync();
            if (result)
                Move2Next();
            else
                break;
        }

        return true;
    }

    bool StopLoop()
    {
        if (Loop)
            return false;

        _timer?.Dispose();
        //_timer.Stop();
        return true;
    }
}

public partial class CarouselxView
{
    PeriodicTimer? _timer;
    readonly uint _animationLength = 800;
    int _currentIndex = 1;
    bool _isAnimating = false;

    void Move2Next()
    {
        if (_mapViews.Count < 3)
            return;

        if (_isAnimating)
            return;

        var currentIndex = _currentIndex;
        var preIndex = (_currentIndex + _mapViews.Count - 1) % _mapViews.Count;
        var prepreIndex = (_currentIndex + _mapViews.Count - 2) % _mapViews.Count;
        var nextIndex = (_currentIndex + 1) % _mapViews.Count;
        var nextnextIndex = (_currentIndex + 2) % _mapViews.Count;

        if (!_mapDockRects.TryGetValue(DockAlignment.Start, out var preRect))
            return;

        if (!_mapDockRects.TryGetValue(DockAlignment.Center, out var centerRect))
            return;

        if (!_mapDockRects.TryGetValue(DockAlignment.End, out var nextRect))
            return;

        if (!_mapDockRects.TryGetValue(DockAlignment.Default, out var defaultRect))
            return;

        _isAnimating = true;

        var animation = new Animation();
        
        if (nextnextIndex == preIndex)
        {
            var preView = _mapViews[preIndex];
            //直接去右边
            var movePreAnimation = new Animation(x =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(preView);
                rect.X = x;
                AbsoluteLayout.SetLayoutBounds(preView, rect);
            }, preRect.Left, nextRect.Left, Easing.SinInOut, finished: () =>
            {
                _mapDockViews[DockAlignment.End] = preView;
            });
            animation.Insert(0, 1, movePreAnimation);
        }
        else
        {
            var preView = _mapViews[preIndex];
            //去背面中间
            var movePreAnimation = new Animation(x =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(preView);
                rect.X = x;
                AbsoluteLayout.SetLayoutBounds(preView, rect);
            }, preRect.Left, defaultRect.Left, Easing.SinInOut, finished: () =>
            {

            });
            animation.Insert(0, 1, movePreAnimation);

            var zIndexAnimation = new Animation(z =>
            {
                preView.ZIndex = 0;
            }, 0, 1);
            animation.Insert(0, 0.1, zIndexAnimation);

            var visiblityAnimation = new Animation(z =>
            {
                preView.IsVisible = false;
            }, 0, 1);
            animation.Insert(0.9, 1, visiblityAnimation);
        }

        //中间左移     
        {
            var centerView = _mapViews[currentIndex];
            var moveCenterAnimation = new Animation(x =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(centerView);
                rect.X = x;
                AbsoluteLayout.SetLayoutBounds(centerView, rect);
            }, centerRect.Left, preRect.Left, Easing.SinInOut, finished: () =>
            {
                _mapDockViews[DockAlignment.Start] = centerView;
            });
            animation.Insert(0, 1, moveCenterAnimation);

            var moveYCenterAnimation = new Animation(y =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(centerView);
                rect.Y = y;
                rect.Height = centerRect.Height - 2 * (y - centerRect.Y);
                AbsoluteLayout.SetLayoutBounds(centerView, rect);
            }, centerRect.Top, preRect.Top, Easing.SinInOut);
            animation.Insert(0, 1, moveYCenterAnimation);

            var zIndexAnimation = new Animation(z =>
            {
                centerView.ZIndex = 1;
            }, 0, 1, Easing.SinInOut);
            animation.Insert(0.6, 0.7, zIndexAnimation);      
        }

        //右边左移 
        {
            var nextView = _mapViews[nextIndex];
            var moveNextAniation = new Animation(x =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(nextView);
                rect.X = x;
                AbsoluteLayout.SetLayoutBounds(nextView, rect);
            }, nextRect.Left, centerRect.Left, Easing.SinInOut, finished: () =>
            {
                _mapDockViews[DockAlignment.Center] = nextView;
            });
            animation.Insert(0, 1, moveNextAniation);

            var moveYNextAnimation = new Animation(y =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(nextView);
                rect.Y = y;
                rect.Height = nextRect.Height + 2 * (nextRect.Y - y);
                AbsoluteLayout.SetLayoutBounds(nextView, rect);
            }, nextRect.Top, centerRect.Top, Easing.SinInOut);
            animation.Insert(0, 1, moveYNextAnimation);

            var zIndexAnimation = new Animation(z =>
            {
                nextView.ZIndex = (int)z;
            }, 1, 2);
            animation.Insert(0.6, 0.7, zIndexAnimation);
        }

        //新的右移  
        if (nextnextIndex != preIndex)
        {
            var nextnextView = _mapViews[nextnextIndex];
            var moveNextNextAnimation = new Animation(x =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(nextnextView);
                rect.X = x;
                AbsoluteLayout.SetLayoutBounds(nextnextView, rect);
            }, defaultRect.Left, nextRect.Left, Easing.SinInOut, finished: () =>
            {
                _mapDockViews[DockAlignment.End] = nextnextView;
            });
            animation.Insert(0, 1, moveNextNextAnimation);

            var zIndexAnimation = new Animation(z =>
            {
                nextnextView.ZIndex = (int)z;
            }, 0, 1);
            animation.Insert(0.6, 0.7, zIndexAnimation);

            var visiblityAnimation = new Animation(z =>
            {
                nextnextView.IsVisible = true;
            }, 0, 1);
            animation.Insert(0, 0.1, visiblityAnimation);
        }

        animation.Commit(this, "MoveAnimation", length: _animationLength, finished: (x, b) =>
        {
            _currentIndex = nextIndex;
            this.CancelAnimations();
            animation.Dispose();
            _isAnimating = false;
        });
    }

    void Move2Pre()
    {
        if (_mapViews.Count < 3)
            return;

        if (_isAnimating)
            return;

        var currentIndex = _currentIndex;
        var preIndex = (_currentIndex + _mapViews.Count - 1) % _mapViews.Count;
        var prepreIndex = (_currentIndex + _mapViews.Count - 2) % _mapViews.Count;
        var nextIndex = (_currentIndex + 1) % _mapViews.Count;
        var nextnextIndex = (_currentIndex + 2) % _mapViews.Count;

        if (!_mapDockRects.TryGetValue(DockAlignment.Start, out var preRect))
            return;

        if (!_mapDockRects.TryGetValue(DockAlignment.Center, out var centerRect))
            return;

        if (!_mapDockRects.TryGetValue(DockAlignment.End, out var nextRect))
            return;

        if (!_mapDockRects.TryGetValue(DockAlignment.Default, out var defaultRect))
            return;

        _isAnimating = true;

        var animation = new Animation();
        if (prepreIndex == nextIndex)
        {
            //直接去左边
            var nextView = _mapViews[nextIndex];
            var moveNextAnimation = new Animation(x =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(nextView);
                rect.X = x;
                AbsoluteLayout.SetLayoutBounds(nextView, rect);
            }, nextRect.Left, preRect.Left, Easing.SinInOut, finished: () =>
            {
                _mapDockViews[DockAlignment.Start] = nextView;
            });
            animation.Insert(0, 1, moveNextAnimation);
        }
        else
        {
            //去背面中间
            var nextView = _mapViews[nextIndex];
            var moveNextAnimation = new Animation(x =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(nextView);
                rect.X = x;
                AbsoluteLayout.SetLayoutBounds(nextView, rect);
            }, nextRect.Left, defaultRect.Left, Easing.SinInOut, finished: () =>
            {

            });
            animation.Insert(0, 1, moveNextAnimation);

            var zIndexAnimation = new Animation(z =>
            {
                nextView.ZIndex = 0;
            }, 0, 1);
            animation.Insert(0, 0.1, zIndexAnimation);

            var visiblityAnimation = new Animation(z =>
            {
                nextView.IsVisible = false;
            }, 0, 1);
            animation.Insert(0.9, 1, visiblityAnimation);
        }

        //中间右移
        var centerView = _mapViews[currentIndex];
        {
            var moveCenterAnimation = new Animation(x =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(centerView);
                rect.X = x;
                AbsoluteLayout.SetLayoutBounds(centerView, rect);
            }, centerRect.Left, nextRect.Left, Easing.SinInOut, finished: () =>
            {
                _mapDockViews[DockAlignment.End] = centerView;
            });
            animation.Insert(0, 1, moveCenterAnimation);

            var moveYCenterAnimation = new Animation(y =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(centerView);
                rect.Y = y;
                rect.Height = centerRect.Height - 2 * (y - centerRect.Y);
                AbsoluteLayout.SetLayoutBounds(centerView, rect);
            }, centerRect.Top, nextRect.Top, Easing.SinInOut);
            animation.Insert(0, 1, moveYCenterAnimation);

            var zIndexAnimation = new Animation(z =>
            {
                centerView.ZIndex = 1;
            }, 0, 1);
            animation.Insert(0.6, 0.7, zIndexAnimation);
        }

        //左边右移 
        var preView = _mapViews[preIndex];
        {
            var movePreAniation = new Animation(x =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(preView);
                rect.X = x;
                AbsoluteLayout.SetLayoutBounds(preView, rect);
            }, preRect.Left, centerRect.Left, Easing.SinInOut, finished: () =>
            {
                _mapDockViews[DockAlignment.Center] = preView;
            });
            animation.Insert(0, 1, movePreAniation);

            var moveYPreAnimation = new Animation(y =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(preView);
                rect.Y = y;
                rect.Height = preRect.Height + 2 * (preRect.Y - y);
                AbsoluteLayout.SetLayoutBounds(preView, rect);
            }, preRect.Top, centerRect.Top, Easing.SinInOut);
            animation.Insert(0, 1, moveYPreAnimation);

            var zIndexAnimation = new Animation(z =>
            {
                preView.ZIndex = (int)z;
            }, 1, 2);
            animation.Insert(0.6, 0.7, zIndexAnimation);
        }

        //新的左移  
        if (prepreIndex != nextIndex)
        {
            var prepreView = _mapViews[prepreIndex];
            var moveNextNextAnimation = new Animation(x =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(prepreView);
                rect.X = x;
                AbsoluteLayout.SetLayoutBounds(prepreView, rect);
            }, defaultRect.Left, preRect.Left, Easing.SinInOut, finished: () =>
            {
                _mapDockViews[DockAlignment.Start] = prepreView;
            });
            animation.Insert(0, 1, moveNextNextAnimation);

            var zIndexAnimation = new Animation(z =>
            {
                prepreView.ZIndex = (int)z;
            }, 0, 1);
            animation.Insert(0.6, 0.7, zIndexAnimation);

            var visiblityAnimation = new Animation(z =>
            {
                prepreView.IsVisible = true;
            }, 0, 1);
            animation.Insert(0, 0.1, visiblityAnimation);
        }

        animation.Commit(this, "MoveAnimation", length: _animationLength, finished: (x, b) =>
        {
            _currentIndex = preIndex;
            this.CancelAnimations();
            animation.Dispose();
            _isAnimating = false;
        });
    }

    void Move2Assign(int index)
    {
        var currentIndex = _currentIndex;
        var preIndex = (_currentIndex + _mapViews.Count - 1) % _mapViews.Count;
        var nextIndex = (_currentIndex + 1) % _mapViews.Count;

        if (index == currentIndex)
            return;

        if (index == preIndex)
        {
            Move2Pre();
            return;
        }
        
        if (index == nextIndex)
        {
            Move2Next();
            return;
        }

        if (_isAnimating)
            return;

        var targetCurrentIndex = index;
        var targetPreIndex = (index + _mapViews.Count - 1) % _mapViews.Count;
        var targetNextIndex = (index + 1) % _mapViews.Count;

        if (!_mapDockRects.TryGetValue(DockAlignment.Start, out var preRect))
            return;

        if (!_mapDockRects.TryGetValue(DockAlignment.Center, out var centerRect))
            return;

        if (!_mapDockRects.TryGetValue(DockAlignment.End, out var nextRect))
            return;

        if (!_mapDockRects.TryGetValue(DockAlignment.Default, out var defaultRect))
            return;

        //先将现在的页面进行合拢

        _isAnimating = true;
        var animation = new Animation();

        {
            _mapDockViews.TryGetValue(DockAlignment.Start, out var preView);
            if (preView is not null)
            {
                var movePreAnimation = new Animation(x =>
                {
                    var rect = AbsoluteLayout.GetLayoutBounds(preView);
                    rect.X = x;
                    AbsoluteLayout.SetLayoutBounds(preView, rect);
                }, preRect.Left, defaultRect.Left, Easing.SinInOut);
                animation.Insert(0, 0.45, movePreAnimation);

                var zIndexAnimation = new Animation(z =>
                {
                    preView.ZIndex = 0;
                    preView.IsVisible = false;
                }, 0, 1);
                animation.Insert(0.4, 0.45, zIndexAnimation);
            }
        }

        {
            _mapDockViews.TryGetValue(DockAlignment.Center, out var centerView);
            if (centerView is not null)
            {
                var moveYCenterAnimation = new Animation(y =>
                {
                    var rect = AbsoluteLayout.GetLayoutBounds(centerView);
                    rect.Y = y;
                    rect.Height = centerRect.Height - 2 * (y - centerRect.Y);
                    AbsoluteLayout.SetLayoutBounds(centerView, rect);
                }, centerRect.Top, nextRect.Top, Easing.SinInOut);
                animation.Insert(0, 0.45, moveYCenterAnimation);

                var zIndexAnimation = new Animation(z =>
                {
                    centerView.ZIndex = 0;
                    centerView.IsVisible = false;
                }, 0, 1);
                animation.Insert(0.4, 0.45, zIndexAnimation);
            }
        }

        {
            _mapDockViews.TryGetValue(DockAlignment.End, out var nextView);
            if (nextView is not null)
            {
                var moveNextAnimation = new Animation(x =>
                {
                    var rect = AbsoluteLayout.GetLayoutBounds(nextView);
                    rect.X = x;
                    AbsoluteLayout.SetLayoutBounds(nextView, rect);
                }, nextRect.Left, defaultRect.Left, Easing.SinInOut);
                animation.Insert(0, 0.45, moveNextAnimation);

                var zIndexAnimation = new Animation(z =>
                {
                    nextView.ZIndex = 0;
                    nextView.IsVisible = false;
                }, 0, 1);
                animation.Insert(0.4, 0.45, zIndexAnimation);
            }
        }

        //再将新页面呈现出来
        {
            var targerPreView = _mapViews[targetPreIndex];
            var zIndexAnimation = new Animation(z =>
            {
                targerPreView.IsVisible = true;
                targerPreView.ZIndex = 1;
            }, 0, 1);
            animation.Insert(0.5, 0.55, zIndexAnimation);

            var movePreAnimation = new Animation(x => 
            {
                var rect = AbsoluteLayout.GetLayoutBounds(targerPreView);
                rect.X = x;
                AbsoluteLayout.SetLayoutBounds(targerPreView, rect);
            }, defaultRect.Left, preRect.Left, Easing.SinInOut, () => 
            {
                _mapDockViews[DockAlignment.Start] = targerPreView;
            });
            animation.Insert(0.55,1, movePreAnimation);
        }

        {
            var targetCenterView = _mapViews[targetCurrentIndex];
            var zIndexAnimation = new Animation(z =>
            {
                targetCenterView.IsVisible = true;
                targetCenterView.ZIndex = 2;
            }, 0, 2);
            animation.Insert(0.5, 0.55, zIndexAnimation);

            var moveYCenterAnimation = new Animation(y =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(targetCenterView);
                rect.Y = y;
                rect.Height = defaultRect.Height + 2 * (defaultRect.Y - y);
                AbsoluteLayout.SetLayoutBounds(targetCenterView, rect);
            }, defaultRect.Top, centerRect.Top, Easing.SinInOut, () =>
            {
                _mapDockViews[DockAlignment.Center] = targetCenterView;
            });
            animation.Insert(0.55, 1, moveYCenterAnimation);
        }

        {
            var targetNextView = _mapViews[targetNextIndex];
            var zIndexAnimation = new Animation(z =>
            {
                targetNextView.IsVisible = true;
                targetNextView.ZIndex = 1;
            }, 0, 1);
            animation.Insert(0.5, 0.55, zIndexAnimation);

            var moveNextAnimation = new Animation(x =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(targetNextView);
                rect.X = x;
                AbsoluteLayout.SetLayoutBounds(targetNextView, rect);
            }, defaultRect.Left, nextRect.Left, Easing.SinInOut, () => 
            {
                _mapDockViews[DockAlignment.End] = targetNextView;
            });
            animation.Insert(0.55, 1, moveNextAnimation);
        }

        animation.Commit(this, "MoveAnimation", length: _animationLength, finished: (x, b) =>
        {
            _currentIndex = index;
            this.CancelAnimations();
            animation.Dispose();
            _isAnimating = false;
        });


    }
}

public partial class CarouselxView
{
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

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
    }

    Command<object>? _selectedCommand = default;
    public ICommand SelectedCommand => _selectedCommand ??= new(t =>
    {
        if (ItemsSource is null)
            return;

        int index = 0;
        int tag = -1;
        foreach (var item in ItemsSource)
        {
            if (t == item)
            {
                tag = index;
                break;
            }

            ++index;
        }

        if (tag < 0)
            return;

        Move2Assign(tag);
    });

    Command<object>? _tapCommand = default;
    public ICommand TapCommand => _tapCommand ??= new(t =>
    {

    });

    Command<object>? _preCommand = default;
    public ICommand PreCommand => _preCommand ??= new(t =>
    {
        Move2Pre();
    });

    Command<object>? _nextCommand = default;
    public ICommand NextCommand => _nextCommand ??= new(t =>
    {
        Move2Next();
    });

    //点击手势
    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {

    }

    //滑动手势
    private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
    {

    }

    //轻扫手势
    private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
    {
        switch (e.Direction)
        {
            case SwipeDirection.Right:
                Move2Next();
                break;
            case SwipeDirection.Left:
                Move2Pre();
                break;
            case SwipeDirection.Up:
                break;
            case SwipeDirection.Down:
                break;
            default:
                break;
        }
    }
}
