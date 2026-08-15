using System.Security.Claims;
using FocusVisk.Application.DTOs;
using FocusVisk.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FocusVisk.API.Controllers;

[ApiController]
[Route("api/notes")]
[Authorize]
public class NotesController : ControllerBase
{
    private readonly INoteService _noteService;
    public NotesController(INoteService noteService) => _noteService = noteService;
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<ActionResult<List<QuickNoteDto>>> GetAll([FromQuery] string? folder, [FromQuery] bool? pinned) =>
        Ok(await _noteService.GetAllAsync(UserId, folder, pinned));

    [HttpGet("{id}")]
    public async Task<ActionResult<QuickNoteDto>> GetById(int id)
    {
        var note = await _noteService.GetByIdAsync(id, UserId);
        return note is null ? NotFound() : Ok(note);
    }

    [HttpPost]
    public async Task<ActionResult<QuickNoteDto>> Create(QuickNoteCreateDto dto) =>
        Ok(await _noteService.CreateAsync(UserId, dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, QuickNoteUpdateDto dto) =>
        await _noteService.UpdateAsync(id, UserId, dto) ? NoContent() : NotFound();

    [HttpPatch("{id}/pin")]
    public async Task<IActionResult> TogglePin(int id)
    {
        var result = await _noteService.TogglePinAsync(id, UserId);
        return result is null ? NotFound() : Ok(new { isPinned = result.Value });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) =>
        await _noteService.DeleteAsync(id, UserId) ? NoContent() : NotFound();
}