using Plugin.Maui.Audio;
using System.Windows.Input;

namespace MauiApp9.ViewModels;

public partial class MainPageViewModel : BasicViewModel
{
    public MainPageViewModel(IAudioManager audiomanager)
    {
        _AudioManager = audiomanager;

        ClickCommand = new Command<object>(async t =>
        {
            var player = _AudioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("startup_sound.mp3"));
            player.Play();
            //player.Dispose();
        });
    }

    readonly IAudioManager _AudioManager;

    public ICommand ClickCommand { get; }

}
