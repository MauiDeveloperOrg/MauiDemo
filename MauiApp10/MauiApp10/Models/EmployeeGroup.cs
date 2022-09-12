using System.Collections.ObjectModel;

namespace MauiApp10.Models;

public partial class EmployeeGroup : ObservableCollection<EmployeeModel>
{
    public EmployeeGroup(string category, List<EmployeeModel> employees, string footer = "") : base(employees)
    {
        Category = category;
        Footer = footer;
    }

    public string Category { get; set; }
    public string Footer { get; set; }
}
