using MauiApp1.ViewModels;

namespace MauiApp1.Views;


public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel mainPageViewModel)
    {
        BindingContext = mainPageViewModel;
        InitializeComponent();
    }


}