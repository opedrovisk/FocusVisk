using AutoMapper;
using FocusVisk.Application.DTOs;
using FocusVisk.Core.Enums;
using FocusVisk.Core.Interfaces;
using FocusVisk.Core.Models;

namespace FocusVisk.Application.Services.Finance;

public class FinanceService : IFinanceService
{
    private readonly IFinanceRepository _repository;
    private readonly IMapper _mapper;

    public FinanceService(IFinanceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<FinancaTransactionDto>> GetTransactionsAsync(string userId, int? month, int? year)
    {
        var transactions = await _repository.GetTransactionsAsync(userId, month, year);
        return _mapper.Map<List<FinancaTransactionDto>>(transactions);
    }

    public async Task<FinancaTransactionDto> CreateTransactionAsync(string userId, FinancaTransactionCreateDto dto)
    {
        var transaction = _mapper.Map<FinancaTransaction>(dto);
        transaction.UserId = userId;
        transaction.CreatedAt = DateTime.UtcNow;

        await _repository.AddTransactionAsync(transaction);
        await _repository.SaveChangesAsync();

        return _mapper.Map<FinancaTransactionDto>(transaction);
    }

    public async Task<bool> UpdateTransactionAsync(int id, string userId, FinancaTransactionUpdateDto dto)
    {
        var transaction = await _repository.GetTransactionByIdAsync(id, userId);
        if (transaction is null) return false;

        _mapper.Map(dto, transaction);
        _repository.UpdateTransaction(transaction);
        return await _repository.SaveChangesAsync();
    }

    public async Task<bool> DeleteTransactionAsync(int id, string userId)
    {
        var transaction = await _repository.GetTransactionByIdAsync(id, userId);
        if (transaction is null) return false;

        _repository.DeleteTransaction(transaction);
        return await _repository.SaveChangesAsync();
    }

    public async Task<SavingGoalDto?> GetGoalAsync(string userId, int month, int year)
    {
        var goal = await _repository.GetGoalAsync(userId, month, year);
        return goal is null ? null : _mapper.Map<SavingGoalDto>(goal);
    }

    public async Task<SavingGoalDto> UpsertGoalAsync(string userId, SavingGoalCreateDto dto)
    {
        var existing = await _repository.GetGoalAsync(userId, dto.MonthYear.Month, dto.MonthYear.Year);

        if (existing is null)
        {
            var goal = _mapper.Map<SavingGoal>(dto);
            goal.UserId = userId;
            await _repository.AddGoalAsync(goal);
            await _repository.SaveChangesAsync();
            return _mapper.Map<SavingGoalDto>(goal);
        }

        existing.Title = dto.Title;
        existing.TargetAmount = dto.TargetAmount;
        _repository.UpdateGoal(existing);
        await _repository.SaveChangesAsync();
        return _mapper.Map<SavingGoalDto>(existing);
    }

    public async Task<FinanceSummaryDto> GetSummaryAsync(string userId, int month, int year)
    {
        var transactions = await _repository.GetTransactionsAsync(userId, month, year);
        var goal = await _repository.GetGoalAsync(userId, month, year);

        var entradas = transactions.Where(t => t.Type == TransactionType.Entrada).Sum(t => t.Amount);
        var saidas = transactions.Where(t => t.Type == TransactionType.Saida).Sum(t => t.Amount);

        return new FinanceSummaryDto
        {
            TotalEntradas = entradas,
            TotalSaidas = saidas,
            Saldo = entradas - saidas,
            MetaDoMes = goal?.TargetAmount
        };
    }
}