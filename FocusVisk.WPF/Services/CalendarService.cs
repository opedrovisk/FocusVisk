using FocusVisk.Data;
using FocusVisk.Models;
using FocusVisk.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class CalendarService
{
    private readonly IServiceProvider _services;
    public event Action? OnChanged;

    public CalendarService(IServiceProvider services) => _services = services;

    private AppDbContext Db() =>
        _services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

    public async Task<List<CalendarNote>> GetForMonthAsync(int year, int month)
    {
        using var db = Db();
        return await db.CalendarNotes
            .Where(n => n.Date.Year == year && n.Date.Month == month)
            .ToListAsync();
    }

    public async Task<List<CalendarNote>> GetForDayAsync(DateTime date)
    {
        using var db = Db();
        return await db.CalendarNotes
            .Where(n => n.Date.Date == date.Date)
            .ToListAsync();
    }
    public async Task<HashSet<DateTime>> GetActiveDatesAsync(int year, int month, TaskService taskSvc)
    {
        var notes = await GetForMonthAsync(year, month);
        var tasks = await taskSvc.GetCalendarTasksForMonthAsync(year, month);

        var dates = notes.Select(n => n.Date.Date)
            .Concat(tasks.Where(t => t.DueDate.HasValue).Select(t => t.DueDate!.Value.Date))
            .ToHashSet();

        return dates;
    }

    public async Task SaveAsync(CalendarNote note)
    {
        using var db = Db();
        if (note.Id == 0) db.CalendarNotes.Add(note);
        else db.CalendarNotes.Update(note);
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }

    public async Task DeleteAsync(int id)
    {
        using var db = Db();
        var note = await db.CalendarNotes.FindAsync(id);
        if (note != null) db.CalendarNotes.Remove(note);
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }
}