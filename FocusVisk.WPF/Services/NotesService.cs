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
}