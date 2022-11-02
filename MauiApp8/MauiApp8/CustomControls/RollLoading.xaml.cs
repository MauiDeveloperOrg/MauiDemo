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
        //PART_Roll1.RotateTo(360, 2000);
        //PART_Roll2.RotateTo(360, 2000);
        //PART_Roll3.RotateTo(360, 2000);
        //PART_Roll4.RotateTo(360, 2000);
        //PART_Roll5.RotateTo(360, 2000);
        //PART_Roll6.RotateTo(360, 2000);
        //PART_Roll7.RotateTo(360, 2000);
        //PART_Roll8.RotateTo(360, 2000);

        var animation1 = new Animation(angle => PART_Roll1.Rotation = angle, 0, 360);
        var animation2 = new Animation(angle => PART_Roll2.Rotation = angle, 0, 360);
        var animation3 = new Animation(angle => PART_Roll3.Rotation = angle, 0, 360);
        var animation4 = new Animation(angle => PART_Roll4.Rotation = angle, 0, 360);
        var animation5 = new Animation(angle => PART_Roll5.Rotation = angle, 0, 360);
        var animation6 = new Animation(angle => PART_Roll6.Rotation = angle, 0, 360);
        var animation7 = new Animation(angle => PART_Roll7.Rotation = angle, 0, 360);
        var animation8 = new Animation(angle => PART_Roll8.Rotation = angle, 0, 360);

        animation1.Commit(PART_Roll1, "Rotate", 16, 1600, EasingX.Linear, (angle, finished) => PART_Roll1.Rotation = 360, () => true);
        animation2.Commit(PART_Roll2, "Rotate", 16, 1400, EasingX.Linear, (angle, finished) => PART_Roll1.Rotation = 360, () => true);
        animation3.Commit(PART_Roll3, "Rotate", 16, 1200, EasingX.Linear, (angle, finished) => PART_Roll1.Rotation = 360, () => true);
        animation4.Commit(PART_Roll4, "Rotate", 16, 1000, EasingX.Linear, (angle, finished) => PART_Roll1.Rotation = 360, () => true);
        animation5.Commit(PART_Roll5, "Rotate", 16, 800, EasingX.Linear, (angle, finished) => PART_Roll1.Rotation = 360, () => true);
        animation6.Commit(PART_Roll6, "Rotate", 16, 600, EasingX.Linear, (angle, finished) => PART_Roll1.Rotation = 360, () => true);
        animation7.Commit(PART_Roll7, "Rotate", 16, 400, EasingX.Linear, (angle, finished) => PART_Roll1.Rotation = 360, () => true);
        animation8.Commit(PART_Roll8, "Rotate", 16, 200, EasingX.Linear, (angle, finished) => PART_Roll1.Rotation = 360, () => true);

    }




    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();





    }

   


}