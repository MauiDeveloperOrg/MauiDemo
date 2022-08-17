namespace MauiApp1;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();   
    }

    protected override bool OnBackButtonPressed()
    {
       
        return base.OnBackButtonPressed();
    }

    protected override void OnNavigating(ShellNavigatingEventArgs args)
    {
        base.OnNavigating(args);
    }

}
