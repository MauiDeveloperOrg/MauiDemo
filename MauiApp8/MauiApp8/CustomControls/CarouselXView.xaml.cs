using System.Collections;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace MauiApp8.CustomControls;

public partial class CarouselXView : TemplatedView
{
	public CarouselXView()
	{
		InitializeComponent();
        WidthRequest = 1000d;
        HeightRequest = 300d;
        var templateObject = GetTemplateChild(nameof(PART_Container));
        if (templateObject is AbsoluteLayout container)
            PART_Container = container;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_Container));

        
    }

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
                                                                  propertyName: nameof(ItemsSource),
                                                                  returnType: typeof(IEnumerable),
                                                                  declaringType: typeof(CarouselXView),
                                                                  defaultValue: default,
                                                                  defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
                                                                   propertyName: nameof(ItemTemplate),
                                                                   returnType: typeof(DataTemplate),
                                                                   declaringType: typeof(CarouselXView),
                                                                   defaultValue: default,
                                                                   defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty EmptyViewProperty = BindableProperty.Create(
                                                                propertyName: nameof(EmptyView),
                                                                returnType: typeof(object),
                                                                declaringType: typeof(CarouselXView),
                                                                defaultValue: default,
                                                                defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty EmptyViewTemplateProperty = BindableProperty.Create(
                                                                        propertyName: nameof(EmptyViewTemplate),
                                                                        returnType: typeof(DataTemplate),
                                                                        declaringType: typeof(CarouselXView),
                                                                        defaultValue: default,
                                                                        defaultBindingMode: BindingMode.TwoWay);

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



     
    readonly AbsoluteLayout PART_Container = default!;

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
        get => (ObservableCollection<View>) GetValue(VisibleViewsProperty); 
    }

    public bool Loop
    {
        get => (bool)GetValue(LoopProperty);
        set => SetValue(LoopProperty, value);
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
    }

    View CreateItemView(object item, DataTemplate dataTemplate)
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