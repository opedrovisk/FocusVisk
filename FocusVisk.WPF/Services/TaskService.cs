using FocusVisk.Data;
using FocusVisk.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace FocusVisk.Services;

public class TaskService
{
    private readonly IServiceProvider _services;
    public event Action? OnChanged;

    public TaskService(IServiceProvider services) => _services = services;

    private AppDbContext Db() =>
        _services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

    public async Task<List<TodoItem>> GetAllAsync()
    {
        using var db = Db();
        return await db.Todos
            .OrderBy(t => t.IsCompleted)
            .ThenByDescending(t => t.Priority)
            .ToListAsync();
    }

    public async Task AddAsync(TodoItem item)
    {
        using var db = Db();
        db.Todos.Add(item);
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }

    public async Task UpdateAsync(TodoItem item)
    {
        using var db = Db();
        db.Todos.Update(item);
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }

    public async Task ToggleAsync(int id)
    {
        using var db = Db();
        var item = await db.Todos.FindAsync(id);
        if (item == null) return;
        item.IsCompleted = !item.IsCompleted;
        item.CompletedAt = item.IsCompleted ? DateTime.Now : null;
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }

    public async Task DeleteAsync(int id)
    {
        using var db = Db();
        var item = await db.Todos.FindAsync(id);
        if (item != null) db.Todos.Remove(item);
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }
}