using FocusVisk.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FocusVisk.Services;

public class TagService
{
    private readonly IServiceProvider _serviceProvider;

    private static readonly string[] Palette =
    {
        "badge-purple",
        "badge-coral",
        "badge-teal",
        "badge-amber",
        "badge-green"
    };

    public TagService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<List<string>> GetAllTagsAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var taskTags = await db.Todos
            .Where(t => t.Tag != null && t.Tag != "")
            .Select(t => t.Tag!)
            .Distinct()
            .ToListAsync();

        var noteTags = await db.QuickNotes
            .Where(n => n.Tag != null && n.Tag != "")
            .Select(n => n.Tag!)
            .Distinct()
            .ToListAsync();

        return taskTags.Concat(noteTags)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(t => t)
            .ToList();
    }

    public static string TagBadgeClass(string? tag)
    {
        if (string.IsNullOrEmpty(tag)) return "badge-purple";
        var hash = 0;
        foreach (var c in tag) hash = hash * 31 + c;
        var idx = Math.Abs(hash) % Palette.Length;
        return Palette[idx];
    }
}