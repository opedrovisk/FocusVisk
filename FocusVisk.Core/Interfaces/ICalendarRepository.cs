using FocusVisk.Core.Models;

namespace FocusVisk.Core.Interfaces;

public interface ICalendarRepository
{
    Task<List<CalendarNote>> GetForMonthAsync(string userId, int year, int month);
    Task<CalendarNote?> GetByIdAsync(int id, string userId);
    Task AddAsync(CalendarNote note);
    void Update(CalendarNote note);
    void Delete(CalendarNote note);
    Task<bool> SaveChangesAsync();
}