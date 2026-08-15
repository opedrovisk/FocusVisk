using FocusVisk.Application.DTOs;
using FocusVisk.Core.Enums;
using FocusVisk.Core.Interfaces;

namespace FocusVisk.Application.Services;

public class SearchService : ISearchService
{
    private readonly ITodoRepository _todoRepository;
    private readonly INoteRepository _noteRepository;
    private readonly IHabitRepository _habitRepository;

    public SearchService(ITodoRepository todoRepository, INoteRepository noteRepository, IHabitRepository habitRepository)
    {
        _todoRepository = todoRepository;
        _noteRepository = noteRepository;
        _habitRepository = habitRepository;
    }

    public async Task<List<SearchResultDto>> SearchAsync(string userId, string query)
    {
        var results = new List<SearchResultDto>();
        var q = query.Trim().ToLower();
        if (q.Length == 0) return results;

        var tasks = await _todoRepository.GetAllAsync(userId);
        results.AddRange(tasks.Where(t => t.Title.ToLower().Contains(q))
            .Select(t => new SearchResultDto { Id = t.Id, Title = t.Title, Subtitle = "Tarefa", Type = SearchResultType.Task, PageTarget = "/tasks" }));

        var notes = await _noteRepository.GetAllAsync(userId, null, null);
        results.AddRange(notes.Where(n => n.Title.ToLower().Contains(q) || n.Content.ToLower().Contains(q))
            .Select(n => new SearchResultDto { Id = n.Id, Title = n.Title, Subtitle = "Nota", Type = SearchResultType.Note, PageTarget = "/notes" }));

        var habits = await _habitRepository.GetAllAsync(userId);
        results.AddRange(habits.Where(h => h.Name.ToLower().Contains(q))
            .Select(h => new SearchResultDto { Id = h.Id, Title = h.Name, Subtitle = "Hábito", Type = SearchResultType.Habit, PageTarget = "/habits" }));

        return results;
    }
}