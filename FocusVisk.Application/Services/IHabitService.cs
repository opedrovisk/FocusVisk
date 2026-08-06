using FocusVisk.Application.DTOs;

namespace FocusVisk.Application.Services;

public interface IHabitService
{
    Task<List<HabitDto>> GetAllAsync(string userId);
    Task<HabitDto> CreateAsync(string userId, HabitCreateDto dto);
    Task<bool> UpdateAsync(int id, string userId, HabitUpdateDto dto);
    Task<bool> DeleteAsync(int id, string userId);
    Task<List<DateTime>> GetLogsForMonthAsync(int habitId, string userId, int year, int month);
    Task<bool?> ToggleLogAsync(int habitId, string userId, DateTime date);
}