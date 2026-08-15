using FocusVisk.Core.Models;

namespace FocusVisk.Core.Interfaces;

public interface INoteRepository
{
    Task<List<QuickNote>> GetAllAsync(string userId, string? folder, bool? pinned);
    Task<QuickNote?> GetByIdAsync(int id, string userId);
    Task AddAsync(QuickNote note);
    void Update(QuickNote note);
    void Delete(QuickNote note);
    Task<bool> SaveChangesAsync();
}