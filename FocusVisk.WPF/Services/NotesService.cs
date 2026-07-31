using FocusVisk.Data;
using FocusVisk.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class NotesService
{
    private readonly IServiceProvider _services;
    public event Action? OnChanged;

    public NotesService(IServiceProvider services) => _services = services;

    private AppDbContext Db() =>
        _services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

    public async Task<List<QuickNote>> GetAllAsync()
    {
        using var db = Db();
        return await db.QuickNotes
            .OrderByDescending(n => n.IsPinned)
            .ThenByDescending(n => n.UpdatedAt)
            .ToListAsync();
    }

    public async Task<List<QuickNote>> GetByFolderAsync(string? folderName)
    {
        using var db = Db();
        var query = db.QuickNotes.AsQueryable();

        if (folderName == "__sem_pasta__")
            query = query.Where(n => n.FolderName == null || n.FolderName == "");
        else if (folderName != null)
            query = query.Where(n => n.FolderName == folderName);

        return await query
            .OrderByDescending(n => n.IsPinned)
            .ThenByDescending(n => n.UpdatedAt)
            .ToListAsync();
    }

    public async Task<List<QuickNote>> SearchByTitleAsync(string search)
    {
        using var db = Db();
        var lower = search.ToLower();
        return await db.QuickNotes
            .Where(n => n.Title.ToLower().Contains(lower))
            .OrderByDescending(n => n.UpdatedAt)
            .ToListAsync();
    }

    public async Task<List<string>> GetFoldersAsync()
    {
        using var db = Db();
        return await db.QuickNotes
            .Where(n => n.FolderName != null && n.FolderName != "")
            .Select(n => n.FolderName!)
            .Distinct()
            .OrderBy(f => f)
            .ToListAsync();
    }

    public async Task SaveAsync(QuickNote note)
    {
        using var db = Db();
        note.UpdatedAt = DateTime.Now;
        if (note.Id == 0) db.QuickNotes.Add(note);
        else db.QuickNotes.Update(note);
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }

    public async Task DeleteAsync(int id)
    {
        using var db = Db();
        var note = await db.QuickNotes.FindAsync(id);
        if (note != null) db.QuickNotes.Remove(note);
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }

    public async Task TogglePinAsync(int id)
    {
        using var db = Db();
        var note = await db.QuickNotes.FindAsync(id);
        if (note == null) return;
        note.IsPinned = !note.IsPinned;
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }

    public async Task RenameFolderAsync(string oldName, string newName)
    {
        using var db = Db();
        var notes = await db.QuickNotes.Where(n => n.FolderName == oldName).ToListAsync();
        foreach (var n in notes) n.FolderName = newName;
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }

    public async Task DeleteFolderAsync(string folderName)
    {
        using var db = Db();
        var notes = await db.QuickNotes.Where(n => n.FolderName == folderName).ToListAsync();
        foreach (var n in notes) n.FolderName = null;
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }
}