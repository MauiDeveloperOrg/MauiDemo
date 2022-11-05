using System.Windows.Input;

namespace MauiApp8.Mvvm;

public class RelayCommand : ICommand
{
    public RelayCommand()
    {

    }

    public event EventHandler? CanExecuteChanged;
    
    bool ICommand.CanExecute(object? parameter)
    {
        throw new NotImplementedException();
    }

    void ICommand.Execute(object? parameter)
    {
        throw new NotImplementedException();
    }
}
