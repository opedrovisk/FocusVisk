using FocusVisk.Core.Enums;

namespace FocusVisk.Application.DTOs;

public class ImportTodoItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public Priority Priority { get; set; }
    public string? Tag { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public TimeSpan? ScheduledTime { get; set; }
    public bool IsRecurring { get; set; }
    public RecurrenceType? RecurrenceType { get; set; }
    public bool ShowInCalendar { get; set; }
    public int? ParentId { get; set; }
}

public class ImportHabitDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public bool IsArchived { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ImportHabitLogDto
{
    public int HabitId { get; set; }
    public DateTime Date { get; set; }
}

public class ImportCalendarNoteDto
{
    public DateTime Date { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? Color { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ImportPomodoroSessionDto
{
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int DurationMinutes { get; set; }
    public bool WasCompleted { get; set; }
    public string? TaskTitle { get; set; }
}

public class ImportQuickNoteDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsPinned { get; set; }
    public string? FolderName { get; set; }
    public string? Tag { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ImportTransactionDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public TransactionCategory Category { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ImportSavingGoalDto
{
    public string Title { get; set; } = string.Empty;
    public decimal TargetAmount { get; set; }
    public DateTime MonthYear { get; set; }
}

public class ImportSettingsDto
{
    public int PomodoroDurationMinutes { get; set; }
    public int ShortBreakMinutes { get; set; }
    public int LongBreakMinutes { get; set; }
    public int SessionsBeforeLongBreak { get; set; }
    public bool PlaySounds { get; set; }
    public bool ShowNotifications { get; set; }
}

public class ImportBackupDto
{
    public int Version { get; set; }
    public DateTime ExportedAt { get; set; }
    public List<ImportTodoItemDto> Todos { get; set; } = new();
    public List<ImportCalendarNoteDto> CalendarNotes { get; set; } = new();
    public List<ImportPomodoroSessionDto> PomodoroSessions { get; set; } = new();
    public List<ImportQuickNoteDto> QuickNotes { get; set; } = new();
    public ImportSettingsDto? Settings { get; set; }
    public List<ImportTransactionDto> Transactions { get; set; } = new();
    public List<ImportSavingGoalDto> SavingGoals { get; set; } = new();
    public List<ImportHabitDto> Habits { get; set; } = new();
    public List<ImportHabitLogDto> HabitLogs { get; set; } = new();
}

public class ImportResultDto
{
    public int TasksImported { get; set; }
    public int HabitsImported { get; set; }
    public int HabitLogsImported { get; set; }
    public int NotesImported { get; set; }
    public int TransactionsImported { get; set; }
    public int SavingGoalsImported { get; set; }
    public int CalendarNotesImported { get; set; }
    public int PomodoroSessionsImported { get; set; }
    public bool SettingsImported { get; set; }
}
