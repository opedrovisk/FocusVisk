using FocusVisk.Core.Interfaces;
using FocusVisk.Core.Models;
using FocusVisk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FocusVisk.Infrastructure.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly AppDbContext _context;

    public TodoRepository(AppDbContext context) => _context = context;

    public async Task<List<TodoItem>> GetAllAsync(string userId) =>
        await _context.Todos
            .Include(t => t.SubTasks)
            .Where(t => t.UserId == userId && t.ParentId == null) 
            .OrderBy(t => t.IsCompleted)
            .ThenBy(t => t.DueDate)
            .ToListAsync();

    public async Task<TodoItem?> GetByIdAsync(int id, string userId) =>
        await _context.Todos
            .Include(t => t.SubTasks)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

    public async Task<List<TodoItem>> GetForMonthAsync(string userId, int year, int month) =>
        await _context.Todos
            .Where(t => t.UserId == userId && t.ShowInCalendar &&
                        t.DueDate != null && t.DueDate.Value.Year == year && t.DueDate.Value.Month == month)
            .ToListAsync();

    public async Task AddAsync(TodoItem task) => await _context.Todos.AddAsync(task);

    public void Update(TodoItem task) => _context.Todos.Update(task);

    public void Delete(TodoItem task) => _context.Todos.Remove(task);

    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
}