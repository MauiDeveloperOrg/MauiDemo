namespace MauiApp8.Easings;
public class EasingX : Easing
{
    static EasingX()
    {
        PowerInOut = new Easing((normalizedTime) =>
        {
            double EaseInCore(double time)
            {
                double y = Math.Max(0.0, 2);
                return Math.Pow(normalizedTime, y);
            }

            if (!(normalizedTime < 0.5))
                return (1.0 - EaseInCore((1.0 - normalizedTime) * 2.0)) * 0.5 + 0.5;

            return EaseInCore(normalizedTime * 2.0) * 0.5;
        });
    }

    public EasingX(Func<double, double> easingFunc) : base(easingFunc)
    {

    }

    public static readonly Easing PowerInOut;

}
