using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using LetsPlayTimer.Services;

namespace LetsPlayTimer.Views;

public partial class MainWindow : Window
{
    private readonly TimerService _timerService = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    public void OnStartClick(object sender, RoutedEventArgs args)
    {
        StartTimerBtn.IsEnabled = false;
        StopTimerBtn.IsEnabled = true;
        _timerService.Start(new TimerConfiguration(
            TimeSpan.FromSeconds((double)(WarnInSecondsNumeric.Value ?? 0)),
            "Assets/ui-warn.wav",
            TimeSpan.FromSeconds((double)(EndInSecondsNumeric.Value ?? 0)),
            "Assets/ui-end.wav"
        ));
    }

    public void OnStopClick(object sender, RoutedEventArgs args)
    {
        StartTimerBtn.IsEnabled = true;
        StopTimerBtn.IsEnabled = false;
        _timerService.Stop();
    }
}
