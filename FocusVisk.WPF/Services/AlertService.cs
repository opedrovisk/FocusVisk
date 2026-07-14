using FocusVisk.Data;
using FocusVisk.Models;
using FocusVisk.Native;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Notifications;

namespace FocusVisk.Services;

public class AlertService : IDisposable
{
    private readonly IServiceProvider _services;
    private readonly System.Threading.Timer _timer;
    private DateTime _lastCheck = DateTime.MinValue;

    public AlertService(IServiceProvider services)
    {
        _services = services;

        _timer = new System.Threading.Timer(
            callback: _ => CheckAlertsAsync().ConfigureAwait(false),
            state: null,
            dueTime: TimeSpan.FromSeconds(5),
            period: TimeSpan.FromSeconds(20)
        );
    }

    private async Task CheckAlertsAsync()
    {
        var now = DateTime.Now;
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
                if (!ShouldFireAlert(task, now)) continue;

                FireToast(task);

                task.LastAlertFiredAt = now;

                if (!task.IsRecurring)
                    task.ScheduledTime = null;

                db.Todos.Update(task);
            }

            await db.SaveChangesAsync();
        }
        catch { }
    }

    private static bool ShouldFireAlert(TodoItem task, DateTime now)
    {
        if (task.ScheduledTime is not TimeSpan time) return false;

        var scheduledToday = now.Date.Add(time);
        var diffMinutes = (now - scheduledToday).TotalMinutes;

        if (diffMinutes < 0 || diffMinutes >= 2)
            return false;

        if (task.LastAlertFiredAt.HasValue &&
            task.LastAlertFiredAt.Value.Date == now.Date &&
            Math.Abs((task.LastAlertFiredAt.Value - scheduledToday).TotalMinutes) < 2)
            return false;

        if (!task.IsRecurring)
        {
            if (task.DueDate.HasValue)
                return task.DueDate.Value.Date <= now.Date;
            return true;
        }

        return task.RecurrenceType switch
        {
            RecurrenceType.Daily => true,
            RecurrenceType.Weekdays => now.DayOfWeek is >= DayOfWeek.Monday and <= DayOfWeek.Friday,
            RecurrenceType.Weekly => task.DueDate.HasValue && now.DayOfWeek == task.DueDate.Value.DayOfWeek,
            RecurrenceType.Monthly => task.DueDate.HasValue && now.Day == task.DueDate.Value.Day,
            _ => false
        };
    }

    private static void FireToast(TodoItem task)
    {
        try
        {
            var recLabel = task.IsRecurring && task.RecurrenceType.HasValue
                ? $" ({RecurrenceLabel(task.RecurrenceType.Value)})"
                : string.Empty;

            var title = $"⏰ triiiimm {task.Title}";
            var body = string.IsNullOrWhiteSpace(task.Description)
                ? $"Hora de realizar sua tarefa!{recLabel}"
                : $"{task.Description}{recLabel}";

            var xml = $"""
                <toast duration="long">
                  <visual>
                    <binding template="ToastGeneric">
                      <text>{EscapeXml(title)}</text>
                      <text>{EscapeXml(body)}</text>
                    </binding>
                  </visual>
                  <audio src="ms-winsoundevent:Notification.Reminder" />
                </toast>
                """;

            var doc = new Windows.Data.Xml.Dom.XmlDocument();
            doc.LoadXml(xml);

            var toast = new ToastNotification(doc);
            ToastNotificationManager.CreateToastNotifier(ToastRegistration.AppId).Show(toast);
        }
        catch { }
    }

    private static string EscapeXml(string text) =>
        text.Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;");

    private static string RecurrenceLabel(RecurrenceType type) => type switch
    {
        RecurrenceType.Daily => "diário",
        RecurrenceType.Weekdays => "dias úteis",
        RecurrenceType.Weekly => "semanal",
        RecurrenceType.Monthly => "mensal",
        _ => ""
    };

    public void Dispose() => _timer.Dispose();
}