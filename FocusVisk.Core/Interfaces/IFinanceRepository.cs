using FocusVisk.Core.Models;

namespace FocusVisk.Core.Interfaces;

public interface IFinanceRepository
{
    // Transações
    Task<List<FinancaTransaction>> GetTransactionsAsync(string userId, int? month, int? year);
    Task<FinancaTransaction?> GetTransactionByIdAsync(int id, string userId);
    Task AddTransactionAsync(FinancaTransaction transaction);
    void UpdateTransaction(FinancaTransaction transaction);
    void DeleteTransaction(FinancaTransaction transaction);

    // Metas
    Task<SavingGoal?> GetGoalAsync(string userId, int month, int year);
    Task AddGoalAsync(SavingGoal goal);
    void UpdateGoal(SavingGoal goal);

    Task<bool> SaveChangesAsync();
}