namespace FocusVisk.Models;

public enum TransactionType { Entrada, Saida }

public enum TransactionCategory
{
    Geral,
    Alimentacao,
    Moradia,
    Transporte,
    Saude,
    Educacao,
    Lazer,
    Salario,
    Investimento,
    Outros
}

public class FinancaTransaction
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public TransactionCategory Category { get; set; } = TransactionCategory.Geral;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class SavingGoal
{
    public int Id { get; set; }
    public string Title { get; set; } = "Meta de economia";
    public decimal TargetAmount { get; set; }
    public DateTime MonthYear { get; set; } = DateTime.Today;
}