using MauiApp2.Models;
using MauiApp2.ViewModels;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewModels;

public partial class MainPageViewModel : BasicViewModel
{
    public MainPageViewModel()
    {
        for (int i = 0; i < 10; i++)
        {
            var student = new Student
            {
                Name = $"张三{i + 1}",
                Number = i + 1,
            };

            Students.Add(student);
        }
    }

    public ObservableCollection<Student> Students { get; } = new();
}
