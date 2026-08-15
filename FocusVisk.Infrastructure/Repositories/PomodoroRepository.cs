using FocusVisk.Core.Interfaces;
using FocusVisk.Core.Models;
using FocusVisk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FocusVisk.Infrastructure.Repositories;

public class PomodoroRepository : IPomodoroRepository
{
    private readonly AppDbContext _context;
    public PomodoroRepository(AppDbContext context) => _context = context;

    public async Task<List<PomodoroSession>> GetSessionsAsync(string userId, DateTime? from, DateTime? to)
    {
        var query = _context.PomodoroSessions.Where(s => s.UserId == userId);
        if (from.HasValue) query = query.Where(s => s.StartedAt >= from.Value);
        if (to.HasValue) query = query.Where(s => s.StartedAt <= to.Value);
        return await query.OrderByDescending(s => s.StartedAt).ToListAsync();
    }

    public async Task AddAsync(PomodoroSession session) => await _context.PomodoroSessions.AddAsync(session);
    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
}