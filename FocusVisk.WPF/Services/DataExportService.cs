using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using FocusVisk.Data;
using FocusVisk.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FocusVisk.Services;

public class BackupData
{
    public int Version { get; set; } = 1;
    public DateTime ExportedAt { get; set; }
    public List<TodoItem> Todos { get; set; } = new();
    public List<CalendarNote> CalendarNotes { get; set; } = new();
    public List<PomodoroSession> PomodoroSessions { get; set; } = new();
    public List<QuickNote> QuickNotes { get; set; } = new();
    public AppSettings? Settings { get; set; }
    public List<FinancaTransaction> Transactions { get; set; } = new();
    public List<SavingGoal> SavingGoals { get; set; } = new();
    public List<Habit> Habits { get; set; } = new();
    public List<HabitLog> HabitLogs { get; set; } = new();
}

public class DataExportService
{
    private readonly IServiceProvider _serviceProvider;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public DataExportService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<string> ExportAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var data = new BackupData
        {
            ExportedAt = DateTime.Now,
            Todos = await db.Todos.AsNoTracking().ToListAsync(),
            CalendarNotes = await db.CalendarNotes.AsNoTracking().ToListAsync(),
            PomodoroSessions = await db.PomodoroSessions.AsNoTracking().ToListAsync(),
            QuickNotes = await db.QuickNotes.AsNoTracking().ToListAsync(),
            Settings = await db.Settings.AsNoTracking().FirstOrDefaultAsync(s => s.Id == 1),
            Transactions = await db.Transactions.AsNoTracking().ToListAsync(),
            SavingGoals = await db.SavingGoals.AsNoTracking().ToListAsync(),
            Habits = await db.Habits.AsNoTracking().ToListAsync(),
            HabitLogs = await db.HabitLogs.AsNoTracking().ToListAsync()
        };

        return JsonSerializer.Serialize(data, JsonOptions);
    }

    public async Task ImportAsync(string json)
    {
        var data = JsonSerializer.Deserialize<BackupData>(json, JsonOptions);
        if (data == null) throw new InvalidOperationException("Arquivo de backup inválido");

        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            db.HabitLogs.RemoveRange(db.HabitLogs);
            db.Transactions.RemoveRange(db.Transactions);
            db.SavingGoals.RemoveRange(db.SavingGoals);
            db.QuickNotes.RemoveRange(db.QuickNotes);
            db.CalendarNotes.RemoveRange(db.CalendarNotes);
            db.PomodoroSessions.RemoveRange(db.PomodoroSessions);
            db.Todos.RemoveRange(db.Todos);
            db.Habits.RemoveRange(db.Habits);
            await db.SaveChangesAsync();

            await ReplaceTableAsync(db, "Habits", data.Habits, () => db.Habits.AddRange(data.Habits));
            await ReplaceTableAsync(db, "HabitLogs", data.HabitLogs, () => db.HabitLogs.AddRange(data.HabitLogs));
            await ReplaceTableAsync(db, "Todos", data.Todos, () => db.Todos.AddRange(data.Todos));
            await ReplaceTableAsync(db, "CalendarNotes", data.CalendarNotes, () => db.CalendarNotes.AddRange(data.CalendarNotes));
            await ReplaceTableAsync(db, "PomodoroSessions", data.PomodoroSessions, () => db.PomodoroSessions.AddRange(data.PomodoroSessions));
            await ReplaceTableAsync(db, "QuickNotes", data.QuickNotes, () => db.QuickNotes.AddRange(data.QuickNotes));
            await ReplaceTableAsync(db, "Transactions", data.Transactions, () => db.Transactions.AddRange(data.Transactions));
            await ReplaceTableAsync(db, "SavingGoals", data.SavingGoals, () => db.SavingGoals.AddRange(data.SavingGoals));

            if (data.Settings != null)
            {
                var existing = await db.Settings.FirstOrDefaultAsync(s => s.Id == 1);
                if (existing != null)
                {
                    db.Entry(existing).CurrentValues.SetValues(data.Settings);
                    db.Entry(existing).Property(s => s.Id).CurrentValue = 1;
                }
                else
                {
                    data.Settings.Id = 1;
                    db.Settings.Add(data.Settings);
                }
                await db.SaveChangesAsync();
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task ReplaceTableAsync<T>(AppDbContext db, string tableName, List<T> items, Action addRange)
    {
        if (items.Count == 0) return;
        await db.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {tableName} ON");
        addRange();
        await db.SaveChangesAsync();
        await db.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {tableName} OFF");
    }
}