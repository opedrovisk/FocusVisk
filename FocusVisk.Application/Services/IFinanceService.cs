using FocusVisk.Application.DTOs;

namespace FocusVisk.Application.Services;

public interface IFinanceService
{
    Task<List<FinancaTransactionDto>> GetTransactionsAsync(string userId, int? month, int? year);
    Task<FinancaTransactionDto> CreateTransactionAsync(string userId, FinancaTransactionCreateDto dto);
    Task<bool> UpdateTransactionAsync(int id, string userId, FinancaTransactionUpdateDto dto);
    Task<bool> DeleteTransactionAsync(int id, string userId);

    Task<SavingGoalDto?> GetGoalAsync(string userId, int month, int year);
    Task<SavingGoalDto> UpsertGoalAsync(string userId, SavingGoalCreateDto dto);

    Task<FinanceSummaryDto> GetSummaryAsync(string userId, int month, int year);
}