using MauiApp10.Models;
using System.Collections.ObjectModel;

namespace MauiApp10.ViewModels;

public partial class MainPageViewModel : BasicViewModel
{
    public MainPageViewModel()
    {
        _EmployeeIns.AddRange(new List<EmployeeModel> {
            new() { FirstName = "Ai", LastName = "Able" },
            new() { FirstName = "Ai", LastName = "Bble" },
            new() { FirstName = "Ai", LastName = "Cble" },
            new() { FirstName = "Bx", LastName = "Able" },
            new() { FirstName = "Bx", LastName = "Bble" },
            new() { FirstName = "Bx", LastName = "Cble" },
            new() { FirstName = "Cj", LastName = "Able" },
            new() { FirstName = "Cj", LastName = "Bble" },
            new() { FirstName = "Cj", LastName = "Cble" },
            new() { FirstName = "Wu", LastName = "Able" },
            new() { FirstName = "Wu", LastName = "Bble" },
            new() { FirstName = "Wu", LastName = "Cble" }
        } );

        var groupDatas = _EmployeeIns.GroupBy(e => e.FirstName).Select(t => 
        {
            if (!string.IsNullOrWhiteSpace(t?.Key))
                return new EmployeeGroup(t.Key, t.ToList());

            return default;
        });

        if (groupDatas is null)
            return;

        Employees.AddRange(groupDatas!);
    }

    List<EmployeeModel> _EmployeeIns = new();
    public List<EmployeeGroup> Employees { get; set; } = new();

}
