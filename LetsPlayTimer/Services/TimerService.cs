using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SoundFlow.Abstracts.Devices;
using SoundFlow.Backends.MiniAudio;
using SoundFlow.Components;
using SoundFlow.Providers;
using SoundFlow.Structs;

namespace LetsPlayTimer.Services;

internal record struct TimerConfiguration(
    TimeSpan WarnAt,
    string WarnSoundFile,
    TimeSpan EndAt,
    string EndSoundFile
);

internal sealed class TimerService
{
    private TimerConfiguration _config;
    private Task? _runningTask;
    private CancellationTokenSource _cancellationTokenSource = new();
    private MiniAudioEngine? _engine;
    private AudioPlaybackDevice? _playbackDevice;
    private SoundPlayer? _player;

    internal void Start(TimerConfiguration config)
    {
        Stop();
        _config = config;
        _runningTask = RunTimerAsync(_cancellationTokenSource.Token);
    }

    internal void Stop()
    {
        if (_runningTask is not null)
        {
            _cancellationTokenSource.Cancel();
            _runningTask = null;
            _cancellationTokenSource = new();
        }
    }

    private async Task RunTimerAsync(CancellationToken cancellationToken)
    {
        do
        {
            await Task.Delay(_config.WarnAt, cancellationToken);
            PlaySound(_config.WarnSoundFile);
            await Task.Delay(_config.EndAt - _config.WarnAt, cancellationToken);
            PlaySound(_config.EndSoundFile);
        }
        while(!cancellationToken.IsCancellationRequested);
    }

    private void PlaySound(string soundPath)
    {
        // Dispose previous playback if any
        _player?.Stop();
        _playbackDevice?.Dispose();
        _engine?.Dispose();

        _engine = new MiniAudioEngine();
        var format = AudioFormat.DvdHq;
        _playbackDevice = _engine.InitializePlaybackDevice(null, format);
        var dataProvider = new StreamDataProvider(_engine, format, File.OpenRead(soundPath));
        _player = new SoundPlayer(_engine, format, dataProvider);
        _playbackDevice.MasterMixer.AddComponent(_player);
        _playbackDevice.Start();
        _player.Play();
    }
}
