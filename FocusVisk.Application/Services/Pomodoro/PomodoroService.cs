using AutoMapper;
using FocusVisk.Application.DTOs;
using FocusVisk.Core.Interfaces;
using FocusVisk.Core.Models;

namespace FocusVisk.Application.Services;

public class PomodoroService : IPomodoroService
{
    private readonly IPomodoroRepository _repository;
    private readonly IMapper _mapper;
    public PomodoroService(IPomodoroRepository repository, IMapper mapper) { _repository = repository; _mapper = mapper; }

    public async Task<List<PomodoroSessionDto>> GetSessionsAsync(string userId, DateTime? from, DateTime? to) =>
        _mapper.Map<List<PomodoroSessionDto>>(await _repository.GetSessionsAsync(userId, from, to));

    public async Task<PomodoroSessionDto> CreateAsync(string userId, PomodoroSessionCreateDto dto)
    {
        var session = _mapper.Map<PomodoroSession>(dto);
        session.UserId = userId;
        await _repository.AddAsync(session);
        await _repository.SaveChangesAsync();
        return _mapper.Map<PomodoroSessionDto>(session);
    }

    public async Task<PomodoroStatsDto> GetStatsAsync(string userId)
    {
        var sessions = await _repository.GetSessionsAsync(userId, null, null);
        return new PomodoroStatsDto
        {
            TotalSessions = sessions.Count,
            CompletedSessions = sessions.Count(s => s.WasCompleted),
            TotalFocusMinutes = sessions.Where(s => s.WasCompleted).Sum(s => s.DurationMinutes)
        };
    }
}