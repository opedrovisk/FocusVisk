using AutoMapper;
using FocusVisk.Application.DTOs;
using FocusVisk.Core.Interfaces;
using FocusVisk.Core.Models;

namespace FocusVisk.Application.Services;

public class NoteService : INoteService
{
    private readonly INoteRepository _repository;
    private readonly IMapper _mapper;
    public NoteService(INoteRepository repository, IMapper mapper) { _repository = repository; _mapper = mapper; }

    public async Task<List<QuickNoteDto>> GetAllAsync(string userId, string? folder, bool? pinned) =>
        _mapper.Map<List<QuickNoteDto>>(await _repository.GetAllAsync(userId, folder, pinned));

    public async Task<QuickNoteDto?> GetByIdAsync(int id, string userId)
    {
        var note = await _repository.GetByIdAsync(id, userId);
        return note is null ? null : _mapper.Map<QuickNoteDto>(note);
    }

    public async Task<QuickNoteDto> CreateAsync(string userId, QuickNoteCreateDto dto)
    {
        var note = _mapper.Map<QuickNote>(dto);
        note.UserId = userId;
        note.CreatedAt = note.UpdatedAt = DateTime.UtcNow;
        await _repository.AddAsync(note);
        await _repository.SaveChangesAsync();
        return _mapper.Map<QuickNoteDto>(note);
    }

    public async Task<bool> UpdateAsync(int id, string userId, QuickNoteUpdateDto dto)
    {
        var note = await _repository.GetByIdAsync(id, userId);
        if (note is null) return false;
        _mapper.Map(dto, note);
        note.UpdatedAt = DateTime.UtcNow;
        _repository.Update(note);
        return await _repository.SaveChangesAsync();
    }

    public async Task<bool?> TogglePinAsync(int id, string userId)
    {
        var note = await _repository.GetByIdAsync(id, userId);
        if (note is null) return null;
        note.IsPinned = !note.IsPinned;
        note.UpdatedAt = DateTime.UtcNow;
        _repository.Update(note);
        await _repository.SaveChangesAsync();
        return note.IsPinned;
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var note = await _repository.GetByIdAsync(id, userId);
        if (note is null) return false;
        _repository.Delete(note);
        return await _repository.SaveChangesAsync();
    }
}