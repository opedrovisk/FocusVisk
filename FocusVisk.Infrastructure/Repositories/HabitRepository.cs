using FocusVisk.Core.Interfaces;
using FocusVisk.Core.Models;
using FocusVisk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FocusVisk.Infrastructure.Repositories;

public class HabitRepository : IHabitRepository
{
    private readonly AppDbContext _context;

    public HabitRepository(AppDbContext context) => _context = context;

    public async Task<List<Habit>> GetAllAsync(string userId) =>
        await _context.Habits
            .Include(h => h.Logs)
            .Where(h => h.UserId == userId && !h.IsArchived)
            .OrderBy(h => h.Name)
            .ToListAsync();

    public async Task<Habit?> GetByIdAsync(int id, string userId) =>
        await _context.Habits
            .Include(h => h.Logs)
            .FirstOrDefaultAsync(h => h.Id == id && h.UserId == userId);

    public async Task<List<DateTime>> GetLogsForMonthAsync(int habitId, int year, int month) =>
        await _context.HabitLogs
            .Where(l => l.HabitId == habitId && l.Date.Year == year && l.Date.Month == month)
            .Select(l => l.Date)
            .ToListAsync();

    public async Task<bool> ToggleLogAsync(int habitId, DateTime date)
    {
        var day = date.Date;
        var existing = await _context.HabitLogs
            .FirstOrDefaultAsync(l => l.HabitId == habitId && l.Date == day);

        if (existing is not null)
        {
            _context.HabitLogs.Remove(existing);
            await _context.SaveChangesAsync();
            return false; // desmarcado
        }

        await _context.HabitLogs.AddAsync(new HabitLog { HabitId = habitId, Date = day });
        await _context.SaveChangesAsync();
        return true; // marcado
    }

    public async Task AddAsync(Habit habit) => await _context.Habits.AddAsync(habit);

    public void Update(Habit habit) => _context.Habits.Update(habit);

    public void Delete(Habit habit) => _context.Habits.Remove(habit);

    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
}