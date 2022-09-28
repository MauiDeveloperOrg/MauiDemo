using CommunityToolkit.Mvvm.Input;
using MauiApp2.Models;
using MauiApp2.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

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

            if (i == 3 || i == 7)
                student.IsShow = true;

            Students.Add(student);
        }

        CheckedCommand = new Command<object>(t =>
        {

        });
    }

    public ObservableCollection<Student> Students { get; } = new();

    public ICommand CheckedCommand { get; }


}
