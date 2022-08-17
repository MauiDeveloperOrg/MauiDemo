using MauiApp2.Views;

namespace MauiApp1;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();   
        Routing.RegisterRoute("LoginRouter", typeof(LoginPage));
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
