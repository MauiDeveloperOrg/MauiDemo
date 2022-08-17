

using MauiApp2.Events;
using Microsoft.Maui.Dispatching;

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
            //Dispatcher.DispatchAsync(()=> TextString = p.Description);
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
            //GoAsync();
            Dispatcher.DispatchAsync(()=>GoAsync());
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var dic = new Dictionary<string, object>();
        dic["parameter1"] = 100;

        await Shell.Current.GoToAsync("LoginRouter", true, dic);
    }

    async void GoAsync()
    {
        var dic = new Dictionary<string, object>();
        dic["parameter2"] = 300;

        await Shell.Current.GoToAsync("TestPage", true, dic);
    }
}