using MauiApp10.ViewModels;

namespace MauiApp10.Views;


public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel mainPageViewModel)
    {
        BindingContext = mainPageViewModel;
        InitializeComponent();
    }


}