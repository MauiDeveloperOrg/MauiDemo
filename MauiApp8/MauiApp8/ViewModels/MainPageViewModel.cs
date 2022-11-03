using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp8.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiApp8.ViewModels;

public partial class MainPageViewModel : BasicViewModel
{
    public MainPageViewModel()
    {

        var random = new Random();
        for (int i = 0; i < 10; i++)
        {
            var student = new Student
            {
                Name = $"张三{i + 1}",
                Number = i + 1,
                Color = Color.FromInt(random.Next())
            };

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
