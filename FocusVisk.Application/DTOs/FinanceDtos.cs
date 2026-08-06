using FocusVisk.Core.Enums;

namespace FocusVisk.Application.DTOs;

public class FinancaTransactionDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public TransactionCategory Category { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class FinancaTransactionCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public TransactionCategory Category { get; set; } = TransactionCategory.Geral;
}

public class FinancaTransactionUpdateDto : FinancaTransactionCreateDto { }

public class SavingGoalDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal TargetAmount { get; set; }
    public DateTime MonthYear { get; set; }
}

public class SavingGoalCreateDto
{
    public string Title { get; set; } = "Meta de economia";
    public decimal TargetAmount { get; set; }
    public DateTime MonthYear { get; set; } = DateTime.UtcNow;
}

public class FinanceSummaryDto
{
    public decimal TotalEntradas { get; set; }
    public decimal TotalSaidas { get; set; }
    public decimal Saldo { get; set; }
    public decimal? MetaDoMes { get; set; }
}