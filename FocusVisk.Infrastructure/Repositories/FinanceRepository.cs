using FocusVisk.Core.Interfaces;
using FocusVisk.Core.Models;
using FocusVisk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FocusVisk.Infrastructure.Repositories;

public class FinanceRepository : IFinanceRepository
{
    private readonly AppDbContext _context;

    public FinanceRepository(AppDbContext context) => _context = context;

    public async Task<List<FinancaTransaction>> GetTransactionsAsync(string userId, int? month, int? year)
    {
        var query = _context.Transactions.Where(t => t.UserId == userId);

        if (month.HasValue)
            query = query.Where(t => t.CreatedAt.Month == month.Value);
        if (year.HasValue)
            query = query.Where(t => t.CreatedAt.Year == year.Value);

        return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
    }

    public async Task<FinancaTransaction?> GetTransactionByIdAsync(int id, string userId) =>
        await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

    public async Task AddTransactionAsync(FinancaTransaction transaction) =>
        await _context.Transactions.AddAsync(transaction);

    public void UpdateTransaction(FinancaTransaction transaction) =>
        _context.Transactions.Update(transaction);

    public void DeleteTransaction(FinancaTransaction transaction) =>
        _context.Transactions.Remove(transaction);

    public async Task<SavingGoal?> GetGoalAsync(string userId, int month, int year) =>
        await _context.SavingGoals.FirstOrDefaultAsync(g =>
            g.UserId == userId && g.MonthYear.Month == month && g.MonthYear.Year == year);

    public async Task AddGoalAsync(SavingGoal goal) =>
        await _context.SavingGoals.AddAsync(goal);

    public void UpdateGoal(SavingGoal goal) =>
        _context.SavingGoals.Update(goal);

    public async Task<bool> SaveChangesAsync() =>
        await _context.SaveChangesAsync() > 0;
}