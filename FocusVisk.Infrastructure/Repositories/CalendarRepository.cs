using FocusVisk.Core.Interfaces;
using FocusVisk.Core.Models;
using FocusVisk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FocusVisk.Infrastructure.Repositories;

public class CalendarRepository : ICalendarRepository
{
    private readonly AppDbContext _context;
    public CalendarRepository(AppDbContext context) => _context = context;

    public async Task<List<CalendarNote>> GetForMonthAsync(string userId, int year, int month) =>
        await _context.CalendarNotes
            .Where(n => n.UserId == userId && n.Date.Year == year && n.Date.Month == month)
            .OrderBy(n => n.Date)
            .ToListAsync();

    public async Task<CalendarNote?> GetByIdAsync(int id, string userId) =>
        await _context.CalendarNotes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

    public async Task AddAsync(CalendarNote note) => await _context.CalendarNotes.AddAsync(note);
    public void Update(CalendarNote note) => _context.CalendarNotes.Update(note);
    public void Delete(CalendarNote note) => _context.CalendarNotes.Remove(note);
    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
}