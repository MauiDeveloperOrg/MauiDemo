using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp8.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiApp8.ViewModels;

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

}
