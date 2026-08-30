using FocusVisk.Core.Interfaces;

namespace FocusVisk.Application.Services;

public class ExportService : IExportService
{
    private readonly ITodoRepository _todoRepository;
    private readonly IHabitRepository _habitRepository;
    private readonly INoteRepository _noteRepository;
    private readonly IFinanceRepository _financeRepository;
    private readonly ICalendarRepository _calendarRepository;

    public ExportService(ITodoRepository todoRepository, IHabitRepository habitRepository,
        INoteRepository noteRepository, IFinanceRepository financeRepository, ICalendarRepository calendarRepository)
    {
        _todoRepository = todoRepository;
        _habitRepository = habitRepository;
        _noteRepository = noteRepository;
        _financeRepository = financeRepository;
        _calendarRepository = calendarRepository;
    }

    public async Task<object> ExportAllAsync(string userId)
    {
        return new
        {
            ExportedAt = DateTime.UtcNow,
            Tasks = await _todoRepository.GetAllAsync(userId),
            Habits = await _habitRepository.GetAllAsync(userId),
            Notes = await _noteRepository.GetAllAsync(userId, null, null),
            Transactions = await _financeRepository.GetTransactionsAsync(userId, null, null)
        };
    }
}
