using FocusVisk.Core.Models;

namespace FocusVisk.Core.Interfaces;

public interface IHabitRepository
{
    Task<List<Habit>> GetAllAsync(string userId);
    Task<Habit?> GetByIdAsync(int id, string userId);
    Task<List<DateTime>> GetLogsForMonthAsync(int habitId, int year, int month);
    Task<bool> ToggleLogAsync(int habitId, DateTime date);
    Task AddAsync(Habit habit);
    void Update(Habit habit);
    void Delete(Habit habit);
    Task<bool> SaveChangesAsync();
}