namespace MauiApp2.Views;

[QueryProperty(nameof(Parameter), "parameter2")]
public partial class TestPage : ContentPage
{
	public TestPage()
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
        }
    }
}