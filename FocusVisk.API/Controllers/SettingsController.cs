using System.Security.Claims;
using FocusVisk.Application.DTOs;
using FocusVisk.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FocusVisk.API.Controllers;

[ApiController]
[Route("api/settings")]
[Authorize]
public class SettingsController : ControllerBase
{
    private readonly ISettingsService _settingsService;
    public SettingsController(ISettingsService settingsService) => _settingsService = settingsService;
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<ActionResult<AppSettingsDto>> Get() => Ok(await _settingsService.GetAsync(UserId));

    [HttpPut]
    public async Task<ActionResult<AppSettingsDto>> Update(AppSettingsUpdateDto dto) =>
        Ok(await _settingsService.UpdateAsync(UserId, dto));
}