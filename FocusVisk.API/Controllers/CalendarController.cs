using FocusVisk.Application.DTOs;
using FocusVisk.Application.Services;
using FocusVisk.Application.Services.Calendar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FocusVisk.API.Controllers;

[ApiController]
[Route("api/calendar")]
[Authorize]
public class CalendarController : ControllerBase
{
    private readonly ICalendarService _calendarService;
    public CalendarController(ICalendarService calendarService) => _calendarService = calendarService;
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<ActionResult<List<CalendarNoteDto>>> GetForMonth([FromQuery] int year, [FromQuery] int month) =>
        Ok(await _calendarService.GetForMonthAsync(UserId, year, month));

    [HttpPost]
    public async Task<ActionResult<CalendarNoteDto>> Create(CalendarNoteCreateDto dto) =>
        Ok(await _calendarService.CreateAsync(UserId, dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CalendarNoteCreateDto dto) =>
        await _calendarService.UpdateAsync(id, UserId, dto) ? NoContent() : NotFound();

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) =>
        await _calendarService.DeleteAsync(id, UserId) ? NoContent() : NotFound();
}