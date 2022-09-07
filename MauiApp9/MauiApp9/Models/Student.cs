﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiApp9.Models;
public partial class Student : BasicModel
{
    [ObservableProperty]
    string? _Name = default;

    [ObservableProperty]
    int _Number = 0;

}
