using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiApp2.Models;
public partial class Student : BasicModel
{
    [ObservableProperty]
    string? _Name = default;

    [ObservableProperty]
    int _Number = 0;

    [ObservableProperty]
    bool _IsShow = false;
}
