using FocusVisk.Core.Models;

namespace FocusVisk.Core.Interfaces;

public interface ITodoRepository
{
    Task<List<TodoItem>> GetAllAsync(string userId);
    Task<TodoItem?> GetByIdAsync(int id, string userId);
    Task<List<TodoItem>> GetForMonthAsync(string userId, int year, int month);
    Task AddAsync(TodoItem task);
    void Update(TodoItem task);
    void Delete(TodoItem task);
    Task<bool> SaveChangesAsync();
}