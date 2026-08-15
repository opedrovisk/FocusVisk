using FocusVisk.Core.Interfaces;
using FocusVisk.Core.Models;
using FocusVisk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FocusVisk.Infrastructure.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly AppDbContext _context;
    public NoteRepository(AppDbContext context) => _context = context;

    public async Task<List<QuickNote>> GetAllAsync(string userId, string? folder, bool? pinned)
    {
        var query = _context.QuickNotes.Where(n => n.UserId == userId);
        if (!string.IsNullOrEmpty(folder)) query = query.Where(n => n.FolderName == folder);
        if (pinned.HasValue) query = query.Where(n => n.IsPinned == pinned.Value);
        return await query.OrderByDescending(n => n.IsPinned).ThenByDescending(n => n.UpdatedAt).ToListAsync();
    }

    public async Task<QuickNote?> GetByIdAsync(int id, string userId) =>
        await _context.QuickNotes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

    public async Task AddAsync(QuickNote note) => await _context.QuickNotes.AddAsync(note);
    public void Update(QuickNote note) => _context.QuickNotes.Update(note);
    public void Delete(QuickNote note) => _context.QuickNotes.Remove(note);
    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
}