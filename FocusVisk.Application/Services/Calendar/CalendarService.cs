using AutoMapper;
using FocusVisk.Application.DTOs;
using FocusVisk.Core.Interfaces;
using FocusVisk.Core.Models;

namespace FocusVisk.Application.Services.Calendar;

public class CalendarService : ICalendarService
{
    private readonly ICalendarRepository _repository;
    private readonly IMapper _mapper;
    public CalendarService(ICalendarRepository repository, IMapper mapper) { _repository = repository; _mapper = mapper; }

    public async Task<List<CalendarNoteDto>> GetForMonthAsync(string userId, int year, int month) =>
        _mapper.Map<List<CalendarNoteDto>>(await _repository.GetForMonthAsync(userId, year, month));

    public async Task<CalendarNoteDto> CreateAsync(string userId, CalendarNoteCreateDto dto)
    {
        var note = _mapper.Map<CalendarNote>(dto);
        note.UserId = userId;
        note.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(note);
        await _repository.SaveChangesAsync();
        return _mapper.Map<CalendarNoteDto>(note);
    }

    public async Task<bool> UpdateAsync(int id, string userId, CalendarNoteCreateDto dto)
    {
        var note = await _repository.GetByIdAsync(id, userId);
        if (note is null) return false;
        _mapper.Map(dto, note);
        _repository.Update(note);
        return await _repository.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var note = await _repository.GetByIdAsync(id, userId);
        if (note is null) return false;
        _repository.Delete(note);
        return await _repository.SaveChangesAsync();
    }
}