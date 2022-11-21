using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp8.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiApp8.ViewModels;

public partial class MainPageViewModel : BasicViewModel
{
    public MainPageViewModel()
    {
        //byte[] bytes = "123132"u8;
        var random = new Random();
        for (int i = 0; i < 10; i++)
        {
            var student = new Student
            {
                Name = $"张三{i + 1}",
                Number = i + 1,
                Color = Color.FromRgb(random.Next(255), random.Next(255), random.Next(255)),
            };

            //if (i == 0)
            //    student.Color = Colors.Red;
            //else if(i == 1)
            //    student.Color = Colors.Blue;
            //else if(i == 2)
            //    student.Color = Colors.Green;
            //else
            //    student.Color = Colors.Yellow;

            Students.Add(student);
        }

        CheckedCommand = new Command<object>(t =>
        {

        });

        TappedCommand = new Command<object>(t =>
        {
            IsInProgress = !IsInProgress;
        });
    }

    public ObservableCollection<Student> Students { get; } = new();

    public ICommand CheckedCommand { get; }

    public ICommand TappedCommand { get; }

    [ObservableProperty]
    public bool _IsInProgress;

    [ObservableProperty]
    public bool _IsGsn;

    [ObservableProperty]
    public bool _IsUser;

}
