using System.Security.Claims;
using FocusVisk.Application.DTOs;
using FocusVisk.Application.Services.Habits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FocusVisk.API.Controllers;

[ApiController]
[Route("api/habits")]
[Authorize]
public class HabitsController : ControllerBase
{
    private readonly IHabitService _habitService;

    public HabitsController(IHabitService habitService) => _habitService = habitService;

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<ActionResult<List<HabitDto>>> GetAll() =>
        Ok(await _habitService.GetAllAsync(UserId));

    [HttpPost]
    public async Task<ActionResult<HabitDto>> Create(HabitCreateDto dto) =>
        Ok(await _habitService.CreateAsync(UserId, dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, HabitUpdateDto dto) =>
        await _habitService.UpdateAsync(id, UserId, dto) ? NoContent() : NotFound();

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) =>
        await _habitService.DeleteAsync(id, UserId) ? NoContent() : NotFound();

    [HttpGet("{id}/logs")]
    public async Task<ActionResult<List<DateTime>>> GetLogs(int id, [FromQuery] int year, [FromQuery] int month) =>
        Ok(await _habitService.GetLogsForMonthAsync(id, UserId, year, month));

    [HttpPost("{id}/toggle")]
    public async Task<IActionResult> ToggleLog(int id, [FromQuery] DateTime date)
    {
        var result = await _habitService.ToggleLogAsync(id, UserId, date);
        return result is null ? NotFound() : Ok(new { marked = result.Value });
    }
}