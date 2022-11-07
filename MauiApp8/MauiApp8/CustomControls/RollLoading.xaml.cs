using MauiApp8.Easings;
using System.Runtime.CompilerServices;

namespace MauiApp8.CustomControls;

public partial class RollLoading : TemplatedView
{
    public RollLoading()
    {
        InitializeComponent();

        Loaded += RollLoading_Loaded;
        SizeChanged += RollLoading_SizeChanged;
    }

    public static readonly BindableProperty RollBrushProperty = BindableProperty.Create(
                                                                propertyName: nameof(RollBrush),
                                                                returnType: typeof(Brush),
                                                                declaringType: typeof(RollLoading),
                                                                defaultValue: new LinearGradientBrush(new()
                                                                {
                                                                    new GradientStop{Color = Colors.Red , Offset = 0.5f},
                                                                    new GradientStop{Color = Colors.Transparent, Offset = 0.5f},
                                                                })
                                                                {
                                                                    StartPoint = new Point(0, 0),
                                                                    EndPoint = new Point(1, 1),
                                                                },
                                                                defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty IsPlayProperty = BindableProperty.Create(
                                                             propertyName: nameof(IsPlay),
                                                             returnType: typeof(bool),
                                                             declaringType: typeof(RollLoading),
                                                             defaultValue: false,
                                                             defaultBindingMode: BindingMode.TwoWay);

    Border PART_Roll1 = default!;
    Border PART_Roll2 = default!;
    Border PART_Roll3 = default!;
    Border PART_Roll4 = default!;
    Border PART_Roll5 = default!;
    Border PART_Roll6 = default!;
    Border PART_Roll7 = default!;
    Border PART_Roll8 = default!;


    List<Animation> Animations { get; } = new();

    public Brush RollBrush
    {
        get => (Brush)GetValue(RollBrushProperty);
        set => SetValue(RollBrushProperty, value);
    }

    public bool IsPlay
    {
        get => (bool)GetValue(IsPlayProperty);
        set => SetValue(IsPlayProperty, value);
    }

    private void RollLoading_Loaded(object? sender, EventArgs e)
    {
        CalculateSize();
        PlayAnimation();
    }
    private void RollLoading_SizeChanged(object? sender, EventArgs e)
    {
        if (!IsLoaded)
            return;
        
        CalculateSize();
        PlayAnimation();
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        var templateObject = GetTemplateChild(nameof(PART_Roll1));
        if (templateObject is Border border1)
            PART_Roll1 = border1;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_Roll1));

        templateObject = GetTemplateChild(nameof(PART_Roll2));
        if (templateObject is Border border2)
            PART_Roll2 = border2;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_Roll2));

        templateObject = GetTemplateChild(nameof(PART_Roll3));
        if (templateObject is Border border3)
            PART_Roll3 = border3;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_Roll3));

        templateObject = GetTemplateChild(nameof(PART_Roll4));
        if (templateObject is Border border4)
            PART_Roll4 = border4;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_Roll4));

        templateObject = GetTemplateChild(nameof(PART_Roll5));
        if (templateObject is Border border5)
            PART_Roll5 = border5;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_Roll5));

        templateObject = GetTemplateChild(nameof(PART_Roll6));
        if (templateObject is Border border6)
            PART_Roll6 = border6;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_Roll6));

        templateObject = GetTemplateChild(nameof(PART_Roll7));
        if (templateObject is Border border7)
            PART_Roll7 = border7;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_Roll7));

        templateObject = GetTemplateChild(nameof(PART_Roll8));
        if (templateObject is Border border8)
            PART_Roll8 = border8;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_Roll8));
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
    }
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        switch (propertyName)
        {
            case nameof(Padding):
            case nameof(IsPlay):
                CalculateSize();
                PlayAnimation();
                break;
            default:
                break;
        }
    }

    bool CalculateSize()
    {
        if (double.IsNaN(Width) || double.IsNaN(Height))
            return false;

        var thickness = Padding;
        if (Width <= thickness.HorizontalThickness || Height <= thickness.VerticalThickness)
            return false;

        double minSize = Math.Min(Width - thickness.HorizontalThickness, Height - thickness.VerticalThickness);
        if (double.IsNaN(minSize) || minSize <= 0)
            return true;

        var size = minSize;
        PART_Roll1.WidthRequest = size;
        PART_Roll1.HeightRequest = size;

        size = minSize * 0.875d;
        PART_Roll2.WidthRequest = size;
        PART_Roll2.HeightRequest = size;

        size = minSize * 0.75d;
        PART_Roll3.WidthRequest = size;
        PART_Roll3.HeightRequest = size;

        size = minSize * 0.625d;
        PART_Roll4.WidthRequest = size;
        PART_Roll4.HeightRequest = size;

        size = minSize * 0.5d;
        PART_Roll5.WidthRequest = size;
        PART_Roll5.HeightRequest = size;

        size = minSize * 0.375d;
        PART_Roll6.WidthRequest = size;
        PART_Roll6.HeightRequest = size;

        size = minSize * 0.25d;
        PART_Roll7.WidthRequest = size;
        PART_Roll7.HeightRequest = size;

        size = minSize * 0.125d;
        PART_Roll8.WidthRequest = size;
        PART_Roll8.HeightRequest = size;

        return true;
    }

    bool PlayAnimation()
    {
        Stop();
        if (!IsPlay)
            return false;

        {
            var animation = new Animation();
            Animations.Add(animation);

            var animation1 = new Animation(angle => PART_Roll1.Rotation = angle, 0, 360);
            animation.Insert(0, 1, animation1);
            var animation2 = new Animation(angle => PART_Roll2.Rotation = angle, 0, 540);
            animation.Insert(0, 1, animation2);
            var animation3 = new Animation(angle => PART_Roll3.Rotation = angle, 0, 720);
            animation.Insert(0, 1, animation3);
            var animation4 = new Animation(angle => PART_Roll4.Rotation = angle, 0, 900);
            animation.Insert(0, 1, animation4);
            var animation5 = new Animation(angle => PART_Roll5.Rotation = angle, 0, 1080);
            animation.Insert(0, 1, animation5);
            var animation6 = new Animation(angle => PART_Roll6.Rotation = angle, 0, 1260);
            animation.Insert(0, 1, animation6);
            var animation7 = new Animation(angle => PART_Roll7.Rotation = angle, 0, 1440);
            animation.Insert(0, 1, animation7);
            var animation8 = new Animation(angle => PART_Roll8.Rotation = angle, 0, 1620);
            animation.Insert(0, 1, animation8);
            animation.Commit(this, "RollRotate", 16, 1600, EasingX.Linear, default, () => true);
        }

        {
            var animation = new Animation();
            Animations.Add(animation);

            var animation1 = new Animation(angle =>
            {
                RollBrush = new LinearGradientBrush(new()
                {
                    new GradientStop{Color = Colors.Red , Offset = 0.5f},
                    new GradientStop{Color = Colors.Transparent, Offset = 0.5f},
                })
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 1),
                };
            }, 0, 1);
            animation.Insert(0, 0.2, animation1);
            var animation2 = new Animation(angle =>
            {
                RollBrush = new LinearGradientBrush(new()
                {
                    new GradientStop{Color = Colors.Aqua , Offset = 0.5f},
                    new GradientStop{Color = Colors.Transparent, Offset = 0.5f},
                })
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 1),
                };
            }, 0, 1);
            animation.Insert(0.2, 0.4, animation2);
            var animation3 = new Animation(angle =>
            {
                RollBrush = new LinearGradientBrush(new()
                {
                    new GradientStop{Color = Colors.Purple , Offset = 0.5f},
                    new GradientStop{Color = Colors.Transparent, Offset = 0.5f},
                })
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 1),
                };
            }, 0, 1);
            animation.Insert(0.4, 0.6, animation3);
            var animation4 = new Animation(angle =>
            {
                RollBrush = new LinearGradientBrush(new()
                {
                    new GradientStop{Color = Colors.Pink , Offset = 0.5f},
                    new GradientStop{Color = Colors.Transparent, Offset = 0.5f},
                })
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 1),
                };
            }, 0, 1);
            animation.Insert(0.6, 0.8, animation4);
            var animation5 = new Animation(angle =>
            {
                RollBrush = new LinearGradientBrush(new()
                {
                    new GradientStop{Color = Colors.Green , Offset = 0.5f},
                    new GradientStop{Color = Colors.Transparent, Offset = 0.5f},
                })
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 1),
                };
            }, 0, 1);

            animation.Insert(0.8, 1, animation5);
            animation.Commit(this, "ColorChange", 16, 1600, EasingX.Linear, default, () => true);
        }

        return true;
    }

    bool Stop()
    {
        this.CancelAnimations();
        foreach (var item in Animations)
            item.Dispose();
        Animations.Clear();

        return true;
    }
}