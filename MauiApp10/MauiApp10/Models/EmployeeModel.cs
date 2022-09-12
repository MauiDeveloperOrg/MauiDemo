using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiApp10.Models;

public partial class EmployeeModel : BaseModel
{
    [ObservableProperty]
    private string? _FirstName;

    [ObservableProperty]
    private string? _LastName;

    public string FullName => $"{FirstName}{LastName}";
}
