using System.Security.Claims;
using FocusVisk.Application.DTOs;
using FocusVisk.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FocusVisk.API.Controllers;

[ApiController]
[Route("api/export")]
[Authorize]
public class ExportController : ControllerBase
{
    private readonly IExportService _exportService;
    private readonly IImportService _importService;

    public ExportController(IExportService exportService, IImportService importService)
    {
        _exportService = exportService;
        _importService = importService;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<IActionResult> Export() => Ok(await _exportService.ExportAllAsync(UserId));

    [HttpPost("import")]
    public async Task<IActionResult> Import(ImportBackupDto backup)
    {
        var result = await _importService.ImportAsync(UserId, backup);
        return Ok(result);
    }
}