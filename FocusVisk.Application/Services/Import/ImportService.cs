using FocusVisk.Application.DTOs;
using FocusVisk.Core.Interfaces;
using FocusVisk.Core.Models;

namespace FocusVisk.Application.Services;

public class ImportService : IImportService
{
    private readonly ITodoRepository _todoRepository;
    private readonly IHabitRepository _habitRepository;
    private readonly INoteRepository _noteRepository;
    private readonly ICalendarRepository _calendarRepository;
    private readonly IPomodoroRepository _pomodoroRepository;
    private readonly IFinanceRepository _financeRepository;
    private readonly ISettingsService _settingsService;

    public ImportService(
        ITodoRepository todoRepository,
        IHabitRepository habitRepository,
        INoteRepository noteRepository,
        ICalendarRepository calendarRepository,
        IPomodoroRepository pomodoroRepository,
        IFinanceRepository financeRepository,
        ISettingsService settingsService)
    {
        _todoRepository = todoRepository;
        _habitRepository = habitRepository;
        _noteRepository = noteRepository;
        _calendarRepository = calendarRepository;
        _pomodoroRepository = pomodoroRepository;
        _financeRepository = financeRepository;
        _settingsService = settingsService;
    }

    public async Task<ImportResultDto> ImportAsync(string userId, ImportBackupDto backup)
    {
        var result = new ImportResultDto();

        // ----- Tarefas: 2 passadas, por causa do ParentId auto-referenciado -----
        var todoIdMap = new Dictionary<int, int>();

        foreach (var dto in backup.Todos)
        {
            var entity = new TodoItem
            {
                UserId = userId,
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
                Priority = dto.Priority,
                Tag = dto.Tag,
                CreatedAt = dto.CreatedAt,
                CompletedAt = dto.CompletedAt,
                DueDate = dto.DueDate,
                ScheduledTime = dto.ScheduledTime,
                IsRecurring = dto.IsRecurring,
                RecurrenceType = dto.RecurrenceType,
                ShowInCalendar = dto.ShowInCalendar
            };

            await _todoRepository.AddAsync(entity);
            await _todoRepository.SaveChangesAsync();

            todoIdMap[dto.Id] = entity.Id;
        }

        foreach (var dto in backup.Todos.Where(t => t.ParentId.HasValue))
        {
            if (!todoIdMap.TryGetValue(dto.Id, out var newChildId)) continue;
            if (!todoIdMap.TryGetValue(dto.ParentId!.Value, out var newParentId)) continue;

            var child = await _todoRepository.GetByIdAsync(newChildId, userId);
            if (child is null) continue;

            child.ParentId = newParentId;
            _todoRepository.Update(child);
        }
        await _todoRepository.SaveChangesAsync();
        result.TasksImported = backup.Todos.Count;

        // ----- Hábitos + Logs -----
        var habitIdMap = new Dictionary<int, int>();

        foreach (var dto in backup.Habits)
        {
            var entity = new Habit
            {
                UserId = userId,
                Name = dto.Name,
                Icon = dto.Icon,
                Color = dto.Color,
                IsArchived = dto.IsArchived,
                CreatedAt = dto.CreatedAt
            };

            await _habitRepository.AddAsync(entity);
            await _habitRepository.SaveChangesAsync();

            habitIdMap[dto.Id] = entity.Id;
        }
        result.HabitsImported = backup.Habits.Count;

        foreach (var logDto in backup.HabitLogs)
        {
            if (!habitIdMap.TryGetValue(logDto.HabitId, out var newHabitId)) continue;
            await _habitRepository.ToggleLogAsync(newHabitId, logDto.Date);
            result.HabitLogsImported++;
        }

        // ----- Entidades simples, sem relação -----
        foreach (var dto in backup.CalendarNotes)
        {
            await _calendarRepository.AddAsync(new CalendarNote
            {
                UserId = userId,
                Date = dto.Date,
                Content = dto.Content,
                Color = dto.Color,
                CreatedAt = dto.CreatedAt
            });
        }
        await _calendarRepository.SaveChangesAsync();
        result.CalendarNotesImported = backup.CalendarNotes.Count;

        foreach (var dto in backup.PomodoroSessions)
        {
            await _pomodoroRepository.AddAsync(new PomodoroSession
            {
                UserId = userId,
                StartedAt = dto.StartedAt,
                CompletedAt = dto.CompletedAt,
                DurationMinutes = dto.DurationMinutes,
                WasCompleted = dto.WasCompleted,
                TaskTitle = dto.TaskTitle
            });
        }
        await _pomodoroRepository.SaveChangesAsync();
        result.PomodoroSessionsImported = backup.PomodoroSessions.Count;

        foreach (var dto in backup.QuickNotes)
        {
            await _noteRepository.AddAsync(new QuickNote
            {
                UserId = userId,
                Title = dto.Title,
                Content = dto.Content,
                IsPinned = dto.IsPinned,
                FolderName = dto.FolderName,
                Tag = dto.Tag,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            });
        }
        await _noteRepository.SaveChangesAsync();
        result.NotesImported = backup.QuickNotes.Count;

        foreach (var dto in backup.Transactions)
        {
            await _financeRepository.AddTransactionAsync(new FinancaTransaction
            {
                UserId = userId,
                Title = dto.Title,
                Description = dto.Description,
                Amount = dto.Amount,
                Type = dto.Type,
                Category = dto.Category,
                CreatedAt = dto.CreatedAt
            });
        }
        await _financeRepository.SaveChangesAsync();
        result.TransactionsImported = backup.Transactions.Count;

        foreach (var dto in backup.SavingGoals)
        {
            await _financeRepository.AddGoalAsync(new SavingGoal
            {
                UserId = userId,
                Title = dto.Title,
                TargetAmount = dto.TargetAmount,
                MonthYear = dto.MonthYear
            });
        }
        await _financeRepository.SaveChangesAsync();
        result.SavingGoalsImported = backup.SavingGoals.Count;

        // ----- Configurações -----
        if (backup.Settings is not null)
        {
            await _settingsService.UpdateAsync(userId, new AppSettingsUpdateDto
            {
                PomodoroDurationMinutes = backup.Settings.PomodoroDurationMinutes,
                ShortBreakMinutes = backup.Settings.ShortBreakMinutes,
                LongBreakMinutes = backup.Settings.LongBreakMinutes,
                SessionsBeforeLongBreak = backup.Settings.SessionsBeforeLongBreak,
                PlaySounds = backup.Settings.PlaySounds,
                ShowNotifications = backup.Settings.ShowNotifications
            });
            result.SettingsImported = true;
        }

        return result;
    }
}
