
using MauiApp2.Events;

namespace MauiApp2.Views;

[QueryProperty(nameof(Parameter), "parameter1")]
public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private int _Parameter1;
    public int Parameter
    {
        get => _Parameter1;
        set
        {
            _Parameter1 = value;
            Label1.Text = Parameter.ToString();
        }
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);      
    }

    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);
    }

    protected override bool OnBackButtonPressed()
    {
        var vFlag=  base.OnBackButtonPressed();

        MessagingCenter.Send<ParameterEvent>(new ParameterEvent 
        {
            Description = "try to send parameterevent message",
        }, nameof(ParameterEvent));

        return vFlag;
    }

}