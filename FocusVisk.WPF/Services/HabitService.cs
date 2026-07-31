using FocusVisk.Data;
using FocusVisk.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class HabitService
{
    private readonly IServiceProvider _services;
    public event Action? OnChanged;

    public HabitService(IServiceProvider services) => _services = services;

    private AppDbContext Db() =>
        _services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

    public async Task<List<Habit>> GetActiveAsync()
    {
        using var db = Db();
        var cutoff = DateTime.Today.AddDays(-29);
        return await db.Habits
            .Where(h => !h.IsArchived)
            .Include(h => h.Logs.Where(l => l.Date >= cutoff))
            .OrderBy(h => h.CreatedAt)
            .ToListAsync();
    }

    public async Task<Habit?> GetByIdAsync(int id)
    {
        using var db = Db();
        return await db.Habits
            .Include(h => h.Logs)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task SaveAsync(Habit habit)
    {
        using var db = Db();
        if (habit.Id == 0) db.Habits.Add(habit);
        else
        {
            var existing = await db.Habits.FindAsync(habit.Id);
            if (existing == null) return;
            existing.Name = habit.Name;
            existing.Icon = habit.Icon;
            existing.Color = habit.Color;
        }
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }

    public async Task ArchiveAsync(int id)
    {
        using var db = Db();
        var habit = await db.Habits.FindAsync(id);
        if (habit == null) return;
        habit.IsArchived = true;
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }

    public async Task DeleteAsync(int id)
    {
        using var db = Db();
        var habit = await db.Habits.FindAsync(id);
        if (habit != null) db.Habits.Remove(habit);
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }

    public async Task ToggleLogAsync(int habitId, DateTime date)
    {
        using var db = Db();
        var dateOnly = date.Date;
        var log = await db.HabitLogs
            .FirstOrDefaultAsync(l => l.HabitId == habitId && l.Date == dateOnly);

        if (log != null)
            db.HabitLogs.Remove(log);
        else
            db.HabitLogs.Add(new HabitLog { HabitId = habitId, Date = dateOnly });

        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }

    public bool IsCompletedToday(Habit habit)
    {
        var today = DateTime.Today;
        return habit.Logs.Any(l => l.Date.Date == today);
    }

    public bool IsCompletedOn(Habit habit, DateTime date)
    {
        return habit.Logs.Any(l => l.Date.Date == date.Date);
    }

    public int GetStreak(Habit habit) => ComputeStreak(habit, DateTime.Today);

    public static int ComputeStreak(Habit habit, DateTime referenceDate)
    {
        var today = referenceDate.Date;
        var logDates = habit.Logs.Select(l => l.Date.Date).ToHashSet();

        if (!logDates.Contains(today) && !logDates.Contains(today.AddDays(-1)))
            return 0;

        var startDate = logDates.Contains(today) ? today : today.AddDays(-1);
        int streak = 0;
        var check = startDate;

        while (logDates.Contains(check))
        {
            streak++;
            check = check.AddDays(-1);
        }

        return streak;
    }

    public int GetLast30DaysCount(Habit habit)
    {
        var cutoff = DateTime.Today.AddDays(-29);
        return habit.Logs.Count(l => l.Date.Date >= cutoff);
    }

    public async Task<HashSet<DateTime>> GetLogsForMonthAsync(int habitId, int year, int month)
    {
        using var db = Db();
        var first = new DateTime(year, month, 1);
        var last = first.AddMonths(1);
        var dates = await db.HabitLogs
            .Where(l => l.HabitId == habitId && l.Date >= first && l.Date < last)
            .Select(l => l.Date.Date)
            .ToListAsync();
        return dates.ToHashSet();
    }
}