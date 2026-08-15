using System.Security.Claims;
using FocusVisk.Application.DTOs;
using FocusVisk.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FocusVisk.API.Controllers;

[ApiController]
[Route("api/pomodoro")]
[Authorize]
public class PomodoroController : ControllerBase
{
    private readonly IPomodoroService _pomodoroService;
    public PomodoroController(IPomodoroService pomodoroService) => _pomodoroService = pomodoroService;
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet("sessions")]
    public async Task<ActionResult<List<PomodoroSessionDto>>> GetSessions([FromQuery] DateTime? from, [FromQuery] DateTime? to) =>
        Ok(await _pomodoroService.GetSessionsAsync(UserId, from, to));

    [HttpPost("sessions")]
    public async Task<ActionResult<PomodoroSessionDto>> Create(PomodoroSessionCreateDto dto) =>
        Ok(await _pomodoroService.CreateAsync(UserId, dto));

    [HttpGet("stats")]
    public async Task<ActionResult<PomodoroStatsDto>> GetStats() =>
        Ok(await _pomodoroService.GetStatsAsync(UserId));
}