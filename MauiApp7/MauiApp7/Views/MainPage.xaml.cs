using SkiaSharp;

namespace MauiApp6.Views;

 
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        
    }

    private void SKCanvasView_PaintSurface(object sender, SkiaSharp.Views.Maui.SKPaintSurfaceEventArgs e)
    {
        //        SKImageInfo info = e.Info;
        //        var surface = e.Surface;
        //        var cavas = surface.Canvas;
        //        cavas.Clear();
        //        const string sksl = @"half4 main(float2 fragCoord)
        //{
        //half4 fragColor = half4(1.0,0.0,0.0,1.0);;

        //return fragColor;
        //}";
        //        //const string sksl = @"sampler2D implicitInput : register(s0);
        //        //float factor : register(c0);
        //        //    float4 main(float2 uv : TEXCOORD) : COLOR
        //        //{

        //        //    float4 color = tex2D(implicitInput, uv);
        //        //    float gray = color.r * 0.3 + color.g * 0.59 + color.b *0.11;  
        //        //    float4 result;    
        //        //    result.r = (color.r - gray) * factor + gray;
        //        //    result.g = (color.g - gray) * factor + gray;
        //        //    result.b = (color.b - gray) * factor + gray;
        //        //    result.a = color.a;
        //        //    return result;
        //        //}";

        //        //        const string sksl = @"void main(float2 fragCoord, inout half4 fragColor)
        //        //{
        //        //    fragColor = half4(1.0,0.0,0.0,1.0);
        //        //}";

        //        using var effect = SKRuntimeEffect.Create(sksl, out var errors);
        //        using var paint = new SKPaint();
        //        var sharder = effect.ToShader(false);
        //        paint.Shader = sharder;
        //        cavas.DrawRect(0, 0, (float)PART_Container.Width, (float)PART_Container.Height, paint);

        SKImageInfo info = e.Info;
        SKSurface surface = e.Surface;
        SKCanvas canvas = surface.Canvas;

        canvas.Clear();

        // Get values from sliders and stepper
        float baseFreqX = (float)Math.Pow(10, 0.03 - 4);
        //baseFrequencyXText.Text = String.Format("Base Frequency X = {0:F4}", baseFreqX);

        float baseFreqY = (float)Math.Pow(10, 0.005 - 4);
        //baseFrequencyYText.Text = String.Format("Base Frequency Y = {0:F4}", baseFreqY);

        int numOctaves = 1;

        using (SKPaint paint = new SKPaint())
        {
            paint.Shader = SKShader.CreateColor(SKColor.Parse("#808080"));

            //paint.Shader =
            //    SKShader.CreatePerlinNoiseFractalNoise(baseFreqX,
            //                                           baseFreqY,
            //                                           numOctaves,
            //                                           0);

            //SKRect rect = new SKRect(0, 0, info.Width, info.Height);
            //canvas.DrawRect(rect, paint);

            //paint.Shader =
            //    SKShader.CreatePerlinNoiseTurbulence(baseFreqX,
            //                                         baseFreqY,
            //                                         numOctaves,
            //                                         0);

            SKRect rect = new SKRect(0, 0, info.Width, info.Height);
            canvas.DrawRect(rect, paint);
        }
    }
}