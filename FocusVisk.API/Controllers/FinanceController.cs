using System.Security.Claims;
using FocusVisk.Application.DTOs;
using FocusVisk.Application.Services.Finance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FocusVisk.API.Controllers;

[ApiController]
[Route("api/finance")]
[Authorize]
public class FinanceController : ControllerBase
{
    private readonly IFinanceService _financeService;

    public FinanceController(IFinanceService financeService) => _financeService = financeService;

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet("transactions")]
    public async Task<ActionResult<List<FinancaTransactionDto>>> GetTransactions([FromQuery] int? month, [FromQuery] int? year) =>
        Ok(await _financeService.GetTransactionsAsync(UserId, month, year));

    [HttpPost("transactions")]
    public async Task<ActionResult<FinancaTransactionDto>> CreateTransaction(FinancaTransactionCreateDto dto) =>
        Ok(await _financeService.CreateTransactionAsync(UserId, dto));

    [HttpPut("transactions/{id}")]
    public async Task<IActionResult> UpdateTransaction(int id, FinancaTransactionUpdateDto dto) =>
        await _financeService.UpdateTransactionAsync(id, UserId, dto) ? NoContent() : NotFound();

    [HttpDelete("transactions/{id}")]
    public async Task<IActionResult> DeleteTransaction(int id) =>
        await _financeService.DeleteTransactionAsync(id, UserId) ? NoContent() : NotFound();

    [HttpGet("goals")]
    public async Task<ActionResult<SavingGoalDto>> GetGoal([FromQuery] int month, [FromQuery] int year)
    {
        var goal = await _financeService.GetGoalAsync(UserId, month, year);
        return goal is null ? NotFound() : Ok(goal);
    }

    [HttpPost("goals")]
    public async Task<ActionResult<SavingGoalDto>> UpsertGoal(SavingGoalCreateDto dto) =>
        Ok(await _financeService.UpsertGoalAsync(UserId, dto));

    [HttpGet("summary")]
    public async Task<ActionResult<FinanceSummaryDto>> GetSummary([FromQuery] int month, [FromQuery] int year) =>
        Ok(await _financeService.GetSummaryAsync(UserId, month, year));
}