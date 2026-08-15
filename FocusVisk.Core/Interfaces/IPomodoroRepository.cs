using FocusVisk.Core.Models;

namespace FocusVisk.Core.Interfaces;

public interface IPomodoroRepository
{
    Task<List<PomodoroSession>> GetSessionsAsync(string userId, DateTime? from, DateTime? to);
    Task AddAsync(PomodoroSession session);
    Task<bool> SaveChangesAsync();
}