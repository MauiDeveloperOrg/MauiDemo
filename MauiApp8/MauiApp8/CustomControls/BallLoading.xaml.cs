using System.Runtime.CompilerServices;

namespace MauiApp8.CustomControls;

public partial class BallLoading : TemplatedView
{
    public BallLoading()
    {
        InitializeComponent();
        Padding = Thickness.Zero;
        HeightRequest = 200d;
        var templateObject = GetTemplateChild(nameof(PART_Container));
        if (templateObject is AbsoluteLayout container)
            PART_Container = container;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_Container));

        templateObject = GetTemplateChild(nameof(PART_Eillipse));
        if (templateObject is Border border)
            PART_Eillipse = border;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_Eillipse));

        templateObject = GetTemplateChild(nameof(PART_Eillipse1));
        if (templateObject is Border border1)
            PART_Eillipse1 = border1;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_Eillipse1));

        templateObject = GetTemplateChild(nameof(PART_Eillipse2));
        if (templateObject is Border border2)
            PART_Eillipse2 = border2;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_Eillipse3));

        templateObject = GetTemplateChild(nameof(PART_Eillipse3));
        if (templateObject is Border border3)
            PART_Eillipse3 = border3;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_Eillipse4));

        templateObject = GetTemplateChild(nameof(PART_Eillipse4));
        if (templateObject is Border border4)
            PART_Eillipse4 = border4;
        else
            ArgumentNullException.ThrowIfNull(nameof(PART_Eillipse4));

        Loaded += BallLoading_Loaded;
        SizeChanged += PARTContainer_SizeChanged;
    }

    public static readonly BindableProperty SpaceProperty = BindableProperty.Create(
                                                            propertyName: nameof(Space),
                                                            returnType: typeof(double),
                                                            declaringType: typeof(BallLoading),
                                                            defaultValue: 40d,
                                                            defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty RunningSpaceProperty = BindableProperty.Create(
                                                                   propertyName: nameof(RunningSpace),
                                                                   returnType: typeof(double),
                                                                   declaringType: typeof(BallLoading),
                                                                   defaultValue: 10d,
                                                                   defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty IsPlayProperty = BindableProperty.Create(
                                                             propertyName: nameof(IsPlay),
                                                             returnType: typeof(bool),
                                                             declaringType: typeof(BallLoading),
                                                             defaultValue: false,
                                                             defaultBindingMode: BindingMode.TwoWay);

    readonly AbsoluteLayout PART_Container = default!;
    readonly Border PART_Eillipse = default!;
    readonly Border PART_Eillipse1= default!;
    readonly Border PART_Eillipse2= default!;
    readonly Border PART_Eillipse3= default!;
    readonly Border PART_Eillipse4 = default!;

    double Count { get; set; } = 5d; 
    double Trip { get; set; } = 0d;
    double Top { get; set; } = 0d;
    double RunningHeight { get; set; } = 0d;
    double Diameter { get; set; } = 0d;

    List<Animation> Animations { get; } = new();

    public double Space
    {
        get => (double)GetValue(SpaceProperty);
        set => SetValue(SpaceProperty, value);
    }

    public double RunningSpace
    {
        get => (double)GetValue(RunningSpaceProperty);
        set => SetValue(RunningSpaceProperty, value);
    }

    public bool IsPlay
    {
        get => (bool)GetValue(IsPlayProperty);
        set => SetValue(IsPlayProperty, value);
    }

    private void PARTContainer_SizeChanged(object? sender, EventArgs e)
    {
        if (!IsLoaded)
            return;

        CalculateSize();
        PlayAnimation();
    }

    private void BallLoading_Loaded(object? sender, EventArgs e)
    {
        CalculateSize();
        PlayAnimation();
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        switch (propertyName)
        {
            case nameof(Space):
            case nameof(RunningSpace):
            case nameof(IsPlay):
            case nameof(Padding):
                CalculateSize();
                PlayAnimation();
                break;
            default:
                break;
        }
    }

    bool CalculateSize()
    {
        double width = PART_Container.Width;
        double height = PART_Container.Height;  
        if (double.IsNaN(height) || double.IsNaN(width))
            return false;

        double minWidth = (width - (Count - 1) * Space) / Count;
        double minHeight = (height -   2 * RunningSpace) / 3d;
        double minSize = Math.Min(minWidth, minHeight);
        if (double.IsNaN(minSize) || minSize <= 0)
            return false;

        Trip = minSize * 4 + Space * 4;
        Top = (height - minSize) / 2d;
        Diameter = minSize;
        RunningHeight = RunningSpace + minSize;

        double left = 0d;
        AbsoluteLayout.SetLayoutBounds(PART_Eillipse, new Rect(left, Top, Diameter, Diameter));
        left = Diameter + Space;
        AbsoluteLayout.SetLayoutBounds(PART_Eillipse1, new Rect(left, Top, Diameter, Diameter));
        left = (Diameter + Space) * 2d;
        AbsoluteLayout.SetLayoutBounds(PART_Eillipse2, new Rect(left, Top, Diameter, Diameter));
        left = (Diameter + Space) * 3d;
        AbsoluteLayout.SetLayoutBounds(PART_Eillipse3, new Rect(left, Top, Diameter, Diameter));
        left = (Diameter + Space) * 4d;
        AbsoluteLayout.SetLayoutBounds(PART_Eillipse4, new Rect(left, Top, Diameter, Diameter));
        return true;
    }

    bool PlayAnimation()
    {
        Stop();
        if (!IsPlay)
            return false;
        var animation = new Animation();
        Animations.Add(animation);
        // 主滚珠 滚动
        {       
            var animation1 = new Animation(distance => AbsoluteLayout.SetLayoutBounds(PART_Eillipse, new Rect(distance, Top, Diameter, Diameter)), 0, Trip);
            animation.Insert(0, 0.5, animation1);

            var animation2 = new Animation(distance => AbsoluteLayout.SetLayoutBounds(PART_Eillipse, new Rect(distance, Top, Diameter, Diameter)), Trip, 0);
            animation.Insert(0.5, 1, animation2);
        }

        double duration = 1d / 16d;
        {
            double distance = Space + Diameter;
            var animation1 = new Animation(leftX =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse1);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse1, new Rect(leftX, rect.Top, Diameter, Diameter));
            }, distance, 0, Easing.CubicInOut);
            animation.Insert(0, duration * 2, animation1);

            var animation2 = new Animation(leftX =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse2);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse2, new Rect(leftX, rect.Top, Diameter, Diameter));
            }, distance * 2, distance, Easing.CubicInOut);
            animation.Insert(duration * 2, duration * 4, animation2);

            var animation3 = new Animation(leftX =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse3);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse3, new Rect(leftX, rect.Top, Diameter, Diameter));
            }, distance * 3, distance * 2, Easing.CubicInOut);
            animation.Insert(duration * 4, duration * 6, animation3);

            var animation4 = new Animation(leftX =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse4);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse4, new Rect(leftX, rect.Top, Diameter, Diameter));
            }, distance * 4, distance * 3, Easing.CubicInOut);
            animation.Insert(duration * 6, duration * 8, animation4);

            var animation5 = new Animation(leftX =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse4);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse4, new Rect(leftX, rect.Top, Diameter, Diameter));
            }, distance * 3, distance * 4, Easing.CubicInOut);
            animation.Insert(duration * 8, duration * 10, animation5);

            var animation6 = new Animation(leftX =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse3);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse3, new Rect(leftX, rect.Top, Diameter, Diameter));
            }, distance * 2, distance * 3, Easing.CubicInOut);
            animation.Insert(duration * 10, duration * 12, animation6);

            var animation7 = new Animation(leftX =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse2);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse2, new Rect(leftX, rect.Top, Diameter, Diameter));
            }, distance , distance * 2, Easing.CubicInOut);
            animation.Insert(duration * 12, duration * 14, animation7);

            var animation8 = new Animation(leftX =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse1);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse1, new Rect(leftX, rect.Top, Diameter, Diameter));
            }, 0, distance, Easing.CubicInOut);
            animation.Insert(duration * 14, duration * 16, animation8);
        }
        // 进程 滚珠绕行
        {
            var animation1 = new Animation(topY =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse1);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse1, new Rect(rect.Left, topY, Diameter, Diameter));
            }, Top, RunningHeight + Top);
            animation.Insert(0, duration, animation1);

            var animation2 = new Animation(topY =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse1);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse1, new Rect(rect.Left, topY, Diameter, Diameter));
            }, RunningHeight + Top, Top);
            animation.Insert(duration, duration * 2, animation2);

            var animation3 = new Animation(topY =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse2);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse2, new Rect(rect.Left, topY, Diameter, Diameter));
            }, Top, RunningHeight + Top);
            animation.Insert(duration * 2, duration * 3, animation3);

            var animation4 = new Animation(topY =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse2);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse2, new Rect(rect.Left, topY, Diameter, Diameter));
            }, RunningHeight + Top, Top);
            animation.Insert(duration * 3, duration * 4, animation4);

            var animation5 = new Animation(topY =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse3);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse3, new Rect(rect.Left, topY, Diameter, Diameter));
            }, Top, RunningHeight + Top);
            animation.Insert(duration * 4, duration * 5, animation5);

            var animation6 = new Animation(topY =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse3);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse3, new Rect(rect.Left, topY, Diameter, Diameter));
            }, RunningHeight + Top, Top);
            animation.Insert(duration * 5, duration * 6, animation6);

            var animation7 = new Animation(topY =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse4);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse4, new Rect(rect.Left, topY, Diameter, Diameter));
            }, Top, RunningHeight + Top);
            animation.Insert(duration * 6, duration * 7, animation7);

            var animation8 = new Animation(topY =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse4);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse4, new Rect(rect.Left, topY, Diameter, Diameter));
            }, RunningHeight + Top, Top);
            animation.Insert(duration * 7, duration * 8, animation8);
        }

        {
            var animation1 = new Animation(topY =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse4);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse4, new Rect(rect.Left, topY, Diameter, Diameter));
            }, Top, Top - RunningHeight);
            animation.Insert(duration * 8, duration * 9, animation1);

            var animation2 = new Animation(topY =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse4);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse4, new Rect(rect.Left, topY, Diameter, Diameter));
            }, Top - RunningHeight, Top);
            animation.Insert(duration * 9, duration * 10, animation2);

            var animation3 = new Animation(topY =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse3);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse3, new Rect(rect.Left, topY, Diameter, Diameter));
            }, Top, Top - RunningHeight);
            animation.Insert(duration * 10, duration * 11, animation3);

            var animation4 = new Animation(topY =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse3);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse3, new Rect(rect.Left, topY, Diameter, Diameter));
            }, Top - RunningHeight, Top);
            animation.Insert(duration * 11, duration * 12, animation4);

            var animation5 = new Animation(topY =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse2);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse2, new Rect(rect.Left, topY, Diameter, Diameter));
            }, Top, Top - RunningHeight);
            animation.Insert(duration * 12, duration * 13, animation5);

            var animation6 = new Animation(topY =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse2);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse2, new Rect(rect.Left, topY, Diameter, Diameter));
            }, Top - RunningHeight, Top);
            animation.Insert(duration * 13, duration * 14, animation6);

            var animation7 = new Animation(topY =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse1);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse1, new Rect(rect.Left, topY, Diameter, Diameter));
            }, Top, Top - RunningHeight);
            animation.Insert(duration * 14, duration * 15, animation7);

            var animation8 = new Animation(topY =>
            {
                var rect = AbsoluteLayout.GetLayoutBounds(PART_Eillipse1);
                AbsoluteLayout.SetLayoutBounds(PART_Eillipse1, new Rect(rect.Left, topY, Diameter, Diameter));
            }, Top - RunningHeight, Top);
            animation.Insert(duration * 15, duration * 16, animation8);

        }

        animation.Commit(this, "Moving", 16, 3000, Easing.Linear, default, () => true);
        Animations.Add(animation);
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