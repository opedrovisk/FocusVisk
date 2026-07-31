using FocusVisk.Data;
using FocusVisk.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FocusVisk.Services;

public class SearchService
{
    private readonly IServiceProvider _serviceProvider;

    public SearchService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<List<SearchResult>> SearchAsync(string query)
    {
        var results = new List<SearchResult>();
        if (string.IsNullOrWhiteSpace(query) || query.Trim().Length < 2)
            return results;

        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var term = query.Trim().ToLower();

        var tasks = await db.Todos
            .Where(t => t.Title.ToLower().Contains(term) || (t.Description != null && t.Description.ToLower().Contains(term)))
            .OrderByDescending(t => t.CreatedAt)
            .Take(6)
            .ToListAsync();

        foreach (var t in tasks)
        {
            results.Add(new SearchResult
            {
                Id = t.Id,
                Title = t.Title,
                Subtitle = t.IsCompleted ? "Concluída" : t.DueDate.HasValue ? $"Vence em {t.DueDate.Value:dd/MM}" : "Tarefa pendente",
                Icon = "fa-solid fa-list-check",
                Type = SearchResultType.Task,
                PageTarget = "tasks"
            });
        }

        var notes = await db.QuickNotes
            .Where(n => n.Title.ToLower().Contains(term) || n.Content.ToLower().Contains(term))
            .OrderByDescending(n => n.UpdatedAt)
            .Take(6)
            .ToListAsync();

        foreach (var n in notes)
        {
            results.Add(new SearchResult
            {
                Id = n.Id,
                Title = n.Title,
                Subtitle = n.FolderName ?? "Sem pasta",
                Icon = "fa-solid fa-note-sticky",
                Type = SearchResultType.Note,
                PageTarget = "notes"
            });
        }

        var habits = await db.Habits
            .Where(h => !h.IsArchived && h.Name.ToLower().Contains(term))
            .Take(6)
            .ToListAsync();

        foreach (var h in habits)
        {
            results.Add(new SearchResult
            {
                Id = h.Id,
                Title = h.Name,
                Subtitle = "Hábito",
                Icon = h.Icon,
                Type = SearchResultType.Habit,
                PageTarget = "habits"
            });
        }

        return results;
    }
}