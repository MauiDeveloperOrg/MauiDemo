using MauiApp9.ViewModels;

namespace MauiApp9.Views;


public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel mainPageViewModel)
    {
        BindingContext = mainPageViewModel;
        InitializeComponent();
    }


}