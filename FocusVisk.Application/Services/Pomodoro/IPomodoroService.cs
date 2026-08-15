using FocusVisk.Application.DTOs;

namespace FocusVisk.Application.Services;

public interface IPomodoroService
{
    Task<List<PomodoroSessionDto>> GetSessionsAsync(string userId, DateTime? from, DateTime? to);
    Task<PomodoroSessionDto> CreateAsync(string userId, PomodoroSessionCreateDto dto);
    Task<PomodoroStatsDto> GetStatsAsync(string userId);
}