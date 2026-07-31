using FocusVisk.Data;
using FocusVisk.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace FocusVisk.Services;

public class FinancasService
{
    private readonly IServiceProvider _services;
    public event Action? OnChanged;

    public FinancasService(IServiceProvider services) => _services = services;

    private AppDbContext Db() =>
        _services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
    public async Task<List<FinancaTransaction>> GetAllAsync()
    {
        using var db = Db();
        return await db.Transactions
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<FinancaTransaction>> GetByMonthAsync(int year, int month)
    {
        using var db = Db();
        return await db.Transactions
            .Where(t => t.CreatedAt.Year == year && t.CreatedAt.Month == month)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(FinancaTransaction t)
    {
        using var db = Db();
        db.Transactions.Add(t);
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }

    public async Task DeleteAsync(int id)
    {
        using var db = Db();
        var item = await db.Transactions.FindAsync(id);
        if (item != null) db.Transactions.Remove(item);
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }
    public async Task<decimal> GetSaldoAsync()
    {
        using var db = Db();
        var all = await db.Transactions.ToListAsync();
        return all.Sum(t => t.Type == TransactionType.Entrada ? t.Amount : -t.Amount);
    }

    public async Task<SavingGoal?> GetGoalAsync(int year, int month)
    {
        using var db = Db();
        return await db.SavingGoals
            .FirstOrDefaultAsync(g => g.MonthYear.Year == year && g.MonthYear.Month == month);
    }

    public async Task SaveGoalAsync(SavingGoal goal)
    {
        using var db = Db();
        var existing = await db.SavingGoals
            .FirstOrDefaultAsync(g => g.MonthYear.Year == goal.MonthYear.Year
                                   && g.MonthYear.Month == goal.MonthYear.Month);
        if (existing == null)
            db.SavingGoals.Add(goal);
        else
        {
            existing.Title = goal.Title;
            existing.TargetAmount = goal.TargetAmount;
            db.SavingGoals.Update(existing);
        }
        await db.SaveChangesAsync();
        OnChanged?.Invoke();
    }
}
