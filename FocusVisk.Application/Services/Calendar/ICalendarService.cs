using FocusVisk.Application.DTOs;

namespace FocusVisk.Application.Services.Calendar;

public interface ICalendarService
{
    Task<List<CalendarNoteDto>> GetForMonthAsync(string userId, int year, int month);
    Task<CalendarNoteDto> CreateAsync(string userId, CalendarNoteCreateDto dto);
    Task<bool> UpdateAsync(int id, string userId, CalendarNoteCreateDto dto);
    Task<bool> DeleteAsync(int id, string userId);
}