using FocusVisk.Application.DTOs;

namespace FocusVisk.Application.Services;

public interface ITaskService
{
    Task<List<TodoItemDto>> GetAllAsync(string userId);
    Task<TodoItemDto?> GetByIdAsync(int id, string userId);
    Task<List<TodoItemDto>> GetForMonthAsync(string userId, int year, int month);
    Task<TodoItemDto> CreateAsync(string userId, TodoItemCreateDto dto);
    Task<bool> UpdateAsync(int id, string userId, TodoItemUpdateDto dto);
    Task<bool> CompleteAsync(int id, string userId);
    Task<bool> DeleteAsync(int id, string userId);
}