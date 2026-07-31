using FocusVisk.Data;
using FocusVisk.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Timers;
using Timer = System.Timers.Timer;

namespace FocusVisk.Services;

public enum PomodoroPhase { Focus, ShortBreak, LongBreak }

public class PomodoroService : IDisposable
{
    private readonly IServiceProvider _services;
    private readonly Timer _timer;

    public event Action<TimeSpan, bool>? OnTick;
    public event Action<PomodoroPhase>? OnPhaseChanged;

    public TimeSpan Remaining { get; private set; }
    public bool IsRunning { get; private set; }
    public PomodoroPhase CurrentPhase { get; private set; } = PomodoroPhase.Focus;
    public int CompletedSessions { get; private set; }
    public int FocusDurationMinutes { get; private set; } = 25;
    public int ShortBreakMinutes { get; private set; } = 5;
    public int LongBreakMinutes { get; private set; } = 15;
    public int SessionsBeforeLongBreak { get; private set; } = 4;

    private DateTime? _sessionStartedAt;
    private string? _currentTaskTitle;

    public PomodoroService(IServiceProvider services)
    {
        _services = services;
        Remaining = TimeSpan.FromMinutes(FocusDurationMinutes);
        _timer = new Timer(1000);
        _timer.Elapsed += OnTimerElapsed;
    }

    public void Start(string? taskTitle = null)
    {
        if (!IsRunning)
        {
            _currentTaskTitle = taskTitle;
            _sessionStartedAt = DateTime.Now;
            IsRunning = true;
            _timer.Start();
        }
    }

    public void Pause()
    {
        IsRunning = false;
        _timer.Stop();
    }

    public void Reset()
    {
        Pause();
        Remaining = TimeSpan.FromMinutes(GetCurrentPhaseDuration());
        OnTick?.Invoke(Remaining, IsRunning);
    }

    public void Skip()
    {
        Pause();
        AdvancePhase(completed: false);
    }

    public void UpdateSettings(int focus, int shortBreak, int longBreak, int sessionsBeforeLong)
    {
        FocusDurationMinutes = focus;
        ShortBreakMinutes = shortBreak;
        LongBreakMinutes = longBreak;
        SessionsBeforeLongBreak = sessionsBeforeLong;
        Reset();
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        Remaining = Remaining.Subtract(TimeSpan.FromSeconds(1));
        OnTick?.Invoke(Remaining, IsRunning);

        if (Remaining <= TimeSpan.Zero)
        {
            _timer.Stop();
            IsRunning = false;
            _ = SaveSessionAsync();
            AdvancePhase(completed: true);
        }
    }

    private void AdvancePhase(bool completed)
    {
        if (CurrentPhase == PomodoroPhase.Focus)
        {
            CompletedSessions++;
            CurrentPhase = CompletedSessions % SessionsBeforeLongBreak == 0
                ? PomodoroPhase.LongBreak
                : PomodoroPhase.ShortBreak;
        }
        else
        {
            CurrentPhase = PomodoroPhase.Focus;
        }

        Remaining = TimeSpan.FromMinutes(GetCurrentPhaseDuration());
        OnPhaseChanged?.Invoke(CurrentPhase);
        OnTick?.Invoke(Remaining, false);
    }

    private int GetCurrentPhaseDuration() => CurrentPhase switch
    {
        PomodoroPhase.Focus => FocusDurationMinutes,
        PomodoroPhase.ShortBreak => ShortBreakMinutes,
        PomodoroPhase.LongBreak => LongBreakMinutes,
        _ => FocusDurationMinutes
    };

    private async Task SaveSessionAsync()
    {
        if (_sessionStartedAt == null) return;
        try
        {
            using var scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.PomodoroSessions.Add(new PomodoroSession
            {
                StartedAt = _sessionStartedAt.Value,
                CompletedAt = DateTime.Now,
                DurationMinutes = FocusDurationMinutes,
                WasCompleted = true,
                TaskTitle = _currentTaskTitle
            });
            await db.SaveChangesAsync();
        }
        catch { }
    }

    public void Dispose()
    {
        _timer.Dispose();
        GC.SuppressFinalize(this);
    }
}