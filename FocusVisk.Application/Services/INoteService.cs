using FocusVisk.Application.DTOs;

namespace FocusVisk.Application.Services;

public interface INoteService
{
    Task<List<QuickNoteDto>> GetAllAsync(string userId, string? folder, bool? pinned);
    Task<QuickNoteDto?> GetByIdAsync(int id, string userId);
    Task<QuickNoteDto> CreateAsync(string userId, QuickNoteCreateDto dto);
    Task<bool> UpdateAsync(int id, string userId, QuickNoteUpdateDto dto);
    Task<bool?> TogglePinAsync(int id, string userId);
    Task<bool> DeleteAsync(int id, string userId);
}