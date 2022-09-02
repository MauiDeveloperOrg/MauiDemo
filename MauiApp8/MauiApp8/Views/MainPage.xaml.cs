using MauiApp8.ViewModels;

namespace MauiApp8.Views;


public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel mainPageViewModel)
    {
        BindingContext = mainPageViewModel;
        InitializeComponent();
    }


}