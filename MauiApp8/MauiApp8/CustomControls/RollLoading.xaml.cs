using MauiApp8.Easings;

namespace MauiApp8.CustomControls;

public partial class RollLoading : Grid
{
    public RollLoading()
    {
        InitializeComponent();
        Loaded += RollLoading_Loaded;
    }

    private void RollLoading_Loaded(object? sender, EventArgs e)
    {
        {
            var animation = new Animation();
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

            //this.CancelAnimations();
        }

        {
            var animation = new Animation();
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
                    new GradientStop{Color = Colors.Blue , Offset = 0.5f},
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
    }

    public static readonly BindableProperty RollColorProperty = BindableProperty.Create(
           propertyName: nameof(RollColor),
           returnType: typeof(Color),
           declaringType: typeof(RollLoading),
           defaultValue: Colors.Red,
           defaultBindingMode: BindingMode.TwoWay,
           propertyChanged: RollColorPropertyChanged);



    public Color RollColor
    {
        get => (Color)GetValue(RollColorProperty);
        set => SetValue(RollColorProperty, value);
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
           defaultBindingMode: BindingMode.TwoWay,
           propertyChanged: RollColorPropertyChanged);



    public Brush RollBrush
    {
        get => (Brush)GetValue(RollBrushProperty);
        set => SetValue(RollBrushProperty, value);
    }


    private static void RollColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {

    }
    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();





    }




}