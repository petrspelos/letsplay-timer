using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using LetsPlayTimer.Services;

namespace LetsPlayTimer.Views;

public partial class MainWindow : Window
{
    private readonly TimerService _timerService = new(new TimerConfiguration(
        TimeSpan.FromSeconds(5),
        "Assets/ui-warn.wav",
        TimeSpan.FromSeconds(10),
        "Assets/ui-end.wav"
    ));

    public MainWindow()
    {
        InitializeComponent();
    }

    public void OnStartClick(object sender, RoutedEventArgs args)
    {
        _timerService.Start();
    }

    public void OnStopClick(object sender, RoutedEventArgs args)
    {
        _timerService.Stop();
    }
}
