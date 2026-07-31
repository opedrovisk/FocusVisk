using FocusVisk.Data;
using FocusVisk.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class TaskService
{
    private readonly IServiceProvider _services;
    public event Action? OnChanged;

    public TaskService(IServiceProvider services) => _services = services;

    private AppDbContext Db() =>
        _services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

    public async Task<List<TodoItem>> GetAllAsync()
    {
        using var db = Db();
        return await db.Todos
            .Where(t => t.ParentId == null)
            .Include(t => t.SubTasks)
            .OrderBy(t => t.IsCompleted)
            .ThenByDescending(t => t.Priority)
            .ThenBy(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<TodoItem>> GetCalendarTasksForMonthAsync(int year, int month)
    {
        using var db = Db();
        var firstDay = new DateTime(year, month, 1);
        var lastDay = firstDay.AddMonths(1).AddDays(-1);

        var tasks = await db.Todos
            .Where(t => t.ShowInCalendar && t.ParentId == null)
            .ToListAsync();

        return tasks.Where(t => AppearsDuringMonth(t, year, month)).ToList();
    }

    public async Task<List<TodoItem>> GetCalendarTasksForDayAsync(DateTime date)
    {
        using var db = Db();
        var tasks = await db.Todos
            .Where(t => t.ShowInCalendar && t.ParentId == null)
            .OrderBy(t => t.IsCompleted)
            .ThenByDescending(t => t.Priority)
            .ToListAsync();

        return tasks.Where(t => AppearsOnDay(t, date)).ToList();
    }

    private static bool AppearsDuringMonth(TodoItem t, int year, int month)
    {
        if (!t.IsRecurring || t.RecurrenceType == null)
        {
            return t.DueDate.HasValue
                && t.DueDate.Value.Year == year
                && t.DueDate.Value.Month == month;
        }

        var firstDay = new DateTime(year, month, 1);
        var lastDay = firstDay.AddMonths(1).AddDays(-1);
        var origin = t.DueDate ?? t.CreatedAt;

        if (origin.Date > lastDay) return false;

        return t.RecurrenceType switch
        {
            RecurrenceType.Daily => true,
            RecurrenceType.Weekdays => true,
            RecurrenceType.Weekly => true,
            RecurrenceType.Monthly => true,
            _ => false
        };
    }

    public static bool AppearsOnDay(TodoItem t, DateTime date)
    {
        if (!t.IsRecurring || t.RecurrenceType == null)
            return t.DueDate.HasValue && t.DueDate.Value.Date == date.Date;

        var origin = (t.DueDate ?? t.CreatedAt).Date;
        if (date.Date < origin) return false;

        return t.RecurrenceType switch
        {
            RecurrenceType.Daily =>
                true,

            RecurrenceType.Weekdays =>
                date.DayOfWeek is >= DayOfWeek.Monday and <= DayOfWeek.Friday,

            RecurrenceType.Weekly =>
                date.DayOfWeek == origin.DayOfWeek,

            RecurrenceType.Monthly =>
                date.Day == origin.Day,

            _ => false
        };
    }

    public async Task AddAsync(TodoItem item)
    {
        using var db = Db();
        db.Todos.Add(item);
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }

    public async Task UpdateAsync(TodoItem item)
    {
        using var db = Db();
        db.Todos.Update(item);
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }

    public async Task DeleteAsync(int id)
    {
        using var db = Db();
        var item = await db.Todos.Include(t => t.SubTasks).FirstOrDefaultAsync(t => t.Id == id);
        if (item == null) return;
        db.Todos.Remove(item);
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }

    public async Task ToggleCompleteAsync(int id)
    {
        using var db = Db();
        var item = await db.Todos.FindAsync(id);
        if (item == null) return;
        item.IsCompleted = !item.IsCompleted;
        item.CompletedAt = item.IsCompleted ? DateTime.Now : null;
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }
    public Task ToggleAsync(int id) => ToggleCompleteAsync(id);
    public Task ToggleSubTaskAsync(int id) => ToggleCompleteAsync(id);

    public async Task ResetRecurringTasksAsync()
    {
        using var db = Db();
        var today = DateTime.Now.Date;

        var recurringTasks = await db.Todos
            .Where(t => t.IsRecurring && t.IsCompleted)
            .ToListAsync();

        var changed = false;

        foreach (var task in recurringTasks)
        {
            if (task.CompletedAt.HasValue && task.CompletedAt.Value.Date == today)
                continue;

            if (AppearsOnDay(task, today))
            {
                task.IsCompleted = false;
                task.CompletedAt = null;
                changed = true;
            }
        }

        if (changed)
            await db.SaveChangesAsync();

        OnChanged?.Invoke();
    }
}