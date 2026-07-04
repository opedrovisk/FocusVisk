using FocusVisk.Data;
using FocusVisk.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FocusVisk.Services;

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
            .ToListAsync();
    }

    public async Task<List<TodoItem>> GetScheduledAsync()
    {
        using var db = Db();
        return await db.Todos
            .Where(t => !t.IsCompleted && t.ScheduledTime != null)
            .OrderBy(t => t.ScheduledTime)
            .ToListAsync();
    }

    public async Task<List<TodoItem>> GetCalendarTasksForMonthAsync(int year, int month)
    {
        using var db = Db();
        return await db.Todos
            .Where(t => t.ShowInCalendar && t.DueDate.HasValue
                        && t.DueDate.Value.Year == year
                        && t.DueDate.Value.Month == month)
            .ToListAsync();
    }
    public async Task<List<TodoItem>> GetCalendarTasksForDayAsync(DateTime date)
    {
        using var db = Db();
        return await db.Todos
            .Where(t => t.ShowInCalendar && t.DueDate.HasValue
                        && t.DueDate.Value.Date == date.Date)
            .OrderBy(t => t.IsCompleted)
            .ThenByDescending(t => t.Priority)
            .ToListAsync();
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

    public async Task ToggleAsync(int id)
    {
        using var db = Db();
        var item = await db.Todos
            .Include(t => t.SubTasks)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (item == null) return;

        item.IsCompleted = !item.IsCompleted;
        item.CompletedAt = item.IsCompleted ? DateTime.Now : null;

        if (item.IsCompleted)
        {
            foreach (var sub in item.SubTasks.Where(s => !s.IsCompleted))
            {
                sub.IsCompleted = true;
                sub.CompletedAt = DateTime.Now;
            }
        }

        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }

    public async Task ToggleSubTaskAsync(int id)
    {
        using var db = Db();
        var item = await db.Todos.FindAsync(id);
        if (item == null) return;
        item.IsCompleted = !item.IsCompleted;
        item.CompletedAt = item.IsCompleted ? DateTime.Now : null;
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }

    public async Task DeleteAsync(int id)
    {
        using var db = Db();

        var subs = await db.Todos.Where(t => t.ParentId == id).ToListAsync();
        db.Todos.RemoveRange(subs);

        var item = await db.Todos.FindAsync(id);
        if (item != null) db.Todos.Remove(item);

        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }
}