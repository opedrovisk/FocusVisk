using FocusVisk.Data;
using FocusVisk.Models;
using H.NotifyIcon;
using H.NotifyIcon.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FocusVisk.Services;
public class AlertService : IDisposable
{
    private readonly IServiceProvider _services;
    private readonly System.Threading.Timer _timer;

    private TaskbarIcon? _trayIcon;

    private DateTime _lastCheck = DateTime.MinValue;

    public AlertService(IServiceProvider services)
    {
        _services = services;

        _timer = new System.Threading.Timer(
            callback: _ => CheckAlertsAsync().ConfigureAwait(false),
            state: null,
            dueTime: TimeSpan.FromSeconds(10),  
            period: TimeSpan.FromSeconds(30)   
        );
    }
    public void SetTrayIcon(TaskbarIcon icon) => _trayIcon = icon;

    private async Task CheckAlertsAsync()
    {
        var now = DateTime.Now;

        if (now.Hour == _lastCheck.Hour && now.Minute == _lastCheck.Minute)
            return;

        _lastCheck = now;

        try
        {
            using var scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var tasks = await db.Todos
                .Where(t => !t.IsCompleted && t.ScheduledTime != null)
                .ToListAsync();

            foreach (var task in tasks)
            {
                if (ShouldFireAlert(task, now))
                {
                    FireNotification(task);

                    task.LastAlertFiredAt = now;

                    if (!task.IsRecurring)
                        task.ScheduledTime = null;

                    db.Todos.Update(task);
                }
            }

            await db.SaveChangesAsync();
        }
        catch
        {
        }
    }

    private static bool ShouldFireAlert(TodoItem task, DateTime now)
    {
        if (task.ScheduledTime is not TimeSpan time) return false;

        if (now.Hour != time.Hours || now.Minute != time.Minutes)
            return false;

        if (task.LastAlertFiredAt.HasValue &&
            task.LastAlertFiredAt.Value.Hour == now.Hour &&
            task.LastAlertFiredAt.Value.Minute == now.Minute &&
            task.LastAlertFiredAt.Value.Date == now.Date)
            return false;

        if (!task.IsRecurring)
        {
            if (task.DueDate.HasValue)
                return task.DueDate.Value.Date <= now.Date;
            return true;
        }

        return task.RecurrenceType switch
        {
            Models.RecurrenceType.Daily => true,
            Models.RecurrenceType.Weekdays => now.DayOfWeek is >= DayOfWeek.Monday
                                                            and <= DayOfWeek.Friday,
            Models.RecurrenceType.Weekly => task.DueDate.HasValue &&
                                              now.DayOfWeek == task.DueDate.Value.DayOfWeek,
            Models.RecurrenceType.Monthly => task.DueDate.HasValue &&
                                              now.Day == task.DueDate.Value.Day,
            _ => false
        };
    }

    private void FireNotification(TodoItem task)
    {
        if (_trayIcon is null) return;

        System.Windows.Application.Current?.Dispatcher.Invoke(() =>
        {
            var recLabel = task.IsRecurring && task.RecurrenceType.HasValue
                ? $" ({RecurrenceLabel(task.RecurrenceType.Value)})"
                : string.Empty;

            var title = $"⏰ FocusVisk — {task.Title}";
            var msg = string.IsNullOrWhiteSpace(task.Description)
                ? $"Hora de cuidar desta tarefa!{recLabel}"
                : $"{task.Description}{recLabel}";

            // API correta do H.NotifyIcon 2.x: ShowNotification(title, msg, NotificationIcon)
            _trayIcon.ShowNotification(title, msg, NotificationIcon.Info);
        });
    }

    private static string RecurrenceLabel(RecurrenceType type) => type switch
    {
        Models.RecurrenceType.Daily => "diário",
        Models.RecurrenceType.Weekdays => "dias úteis",
        Models.RecurrenceType.Weekly => "semanal",
        Models.RecurrenceType.Monthly => "mensal",
        _ => ""
    };

    public void Dispose() => _timer.Dispose();
}