

using MauiApp2.Events;

namespace MauiApp1.Views;

[QueryProperty(nameof(TextString), "description")]
public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
        MessagingCenter.Subscribe<ParameterEvent>(this, nameof(ParameterEvent), p =>
        {
            TextString = p.Description;
        });
    }

    private string? _TextString;
    public string? TextString
    {
        get => _TextString;
        set
        {
            _TextString = value;
            PART_Label.Text = value;
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var dic = new Dictionary<string, object>();
        dic["parameter1"] = 100;

        await Shell.Current.GoToAsync("LoginRouter", true, dic);
    }
}