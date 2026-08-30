using System.Security.Claims;
using FocusVisk.Application.DTOs;
using FocusVisk.Application.Services.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FocusVisk.API.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService) => _taskService = taskService;

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<ActionResult<List<TodoItemDto>>> GetAll() =>
        Ok(await _taskService.GetAllAsync(UserId));

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDto>> GetById(int id)
    {
        var task = await _taskService.GetByIdAsync(id, UserId);
        return task is null ? NotFound() : Ok(task);
    }

    [HttpGet("calendar")]
    public async Task<ActionResult<List<TodoItemDto>>> GetForMonth([FromQuery] int year, [FromQuery] int month) =>
        Ok(await _taskService.GetForMonthAsync(UserId, year, month));

    [HttpPost]
    public async Task<ActionResult<TodoItemDto>> Create(TodoItemCreateDto dto)
    {
        var created = await _taskService.CreateAsync(UserId, dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TodoItemUpdateDto dto) =>
        await _taskService.UpdateAsync(id, UserId, dto) ? NoContent() : NotFound();

    [HttpPatch("{id}/complete")]
    public async Task<IActionResult> Complete(int id) =>
        await _taskService.CompleteAsync(id, UserId) ? NoContent() : NotFound();

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) =>
        await _taskService.DeleteAsync(id, UserId) ? NoContent() : NotFound();
}