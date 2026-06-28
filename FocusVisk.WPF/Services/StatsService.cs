using FocusVisk.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class StatsService
{
    private readonly IServiceProvider _services;

    public StatsService(IServiceProvider services) => _services = services;

    private AppDbContext Db() =>
        _services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

    public async Task<DashboardStats> GetStatsAsync()
    {
        using var db = Db();
        var now = DateTime.Now;
        var weekStart = now.AddDays(-(int)now.DayOfWeek);

        var todaySessions = await db.PomodoroSessions
            .CountAsync(s => s.StartedAt.Date == now.Date && s.WasCompleted);

        var weekSessions = await db.PomodoroSessions
            .CountAsync(s => s.StartedAt >= weekStart && s.WasCompleted);

        var completedToday = await db.Todos
            .CountAsync(t => t.CompletedAt.HasValue && t.CompletedAt.Value.Date == now.Date);

        var pendingTasks = await db.Todos.CountAsync(t => !t.IsCompleted);

        var streak = await CalculateStreakAsync(db);

        var ptBR = new System.Globalization.CultureInfo("pt-BR");
        var dailySessions = new List<DailyStat>();
        for (int i = 6; i >= 0; i--)
        {
            var day = now.AddDays(-i).Date;
            var count = await db.PomodoroSessions
                .CountAsync(s => s.StartedAt.Date == day && s.WasCompleted);
            dailySessions.Add(new DailyStat
            {
                Day = day.ToString("ddd", ptBR),
                Count = count
            });
        }

        return new DashboardStats
        {
            TodaySessions = todaySessions,
            WeekSessions = weekSessions,
            CompletedToday = completedToday,
            PendingTasks = pendingTasks,
            Streak = streak,
            DailyStats = dailySessions
        };
    }

    private async Task<int> CalculateStreakAsync(AppDbContext db)
    {
        var streak = 0;
        for (int i = 0; i < 365; i++)
        {
            var d = DateTime.Now.Date.AddDays(-i);
            var has = await db.PomodoroSessions
                .AnyAsync(s => s.StartedAt.Date == d && s.WasCompleted);
            if (!has) break;
            streak++;
        }
        return streak;
    }
}

public class DashboardStats
{
    public int TodaySessions { get; set; }
    public int WeekSessions { get; set; }
    public int CompletedToday { get; set; }
    public int PendingTasks { get; set; }
    public int Streak { get; set; }
    public List<DailyStat> DailyStats { get; set; } = new();
}

public class DailyStat
{
    public string Day { get; set; } = string.Empty;
    public int Count { get; set; }
}