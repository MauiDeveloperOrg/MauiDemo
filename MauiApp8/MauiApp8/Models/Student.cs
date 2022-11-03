using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiApp8.Models;
public partial class Student : BasicModel
{
    [ObservableProperty]
    string? _Name = default;

    [ObservableProperty]
    int _Number = 0;

    [ObservableProperty]
    Color? _Color = default;
}
