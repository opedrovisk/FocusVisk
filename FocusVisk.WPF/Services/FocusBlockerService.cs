using FocusVisk.Data;
using FocusVisk.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

public class FocusBlockerService
{
    private const string HostsPath = @"C:\Windows\System32\drivers\etc\hosts";
    private const string BlockMarkerStart = "# === FOCUS FocusVisk START ===";
    private const string BlockMarkerEnd = "# === FOCUS FocusVisk END ===";

    private readonly IServiceProvider _sp;
    public bool IsBlocking { get; private set; }

    public FocusBlockerService(IServiceProvider sp) => _sp = sp;
    public async Task<string> GetBlockedSitesAsync()
    {
        using var scope = _sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var settings = await db.Settings.FirstOrDefaultAsync(s => s.Id == 1);
        return settings?.BlockedSites ?? string.Empty;
    }
    public async Task SaveBlockedSitesAsync(string sites)
    {
        using var scope = _sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var settings = await db.Settings.FirstOrDefaultAsync(s => s.Id == 1);
        if (settings is null) return;
        settings.BlockedSites = sites;
        await db.SaveChangesAsync();
    }

    public async Task BlockAsync(IEnumerable<string> sites)
    {
        try
        {
            await UnblockAsync();
            var lines = new List<string> { BlockMarkerStart };
            foreach (var site in sites)
            {
                var clean = site.Trim().ToLower();
                if (string.IsNullOrWhiteSpace(clean)) continue;
                lines.Add($"127.0.0.1 {clean}");
                lines.Add($"127.0.0.1 www.{clean}");
            }
            lines.Add(BlockMarkerEnd);
            await File.AppendAllLinesAsync(HostsPath, lines);
            IsBlocking = true;
        }
        catch (UnauthorizedAccessException)
        {
            throw new InvalidOperationException(
                "Para bloquear sites, execute o FocusVisk como Administrador.");
        }
    }

    public async Task UnblockAsync()
    {
        if (!File.Exists(HostsPath)) return;
        var content = await File.ReadAllTextAsync(HostsPath);
        var startIdx = content.IndexOf(BlockMarkerStart, StringComparison.Ordinal);
        var endIdx = content.IndexOf(BlockMarkerEnd, StringComparison.Ordinal);
        if (startIdx >= 0 && endIdx >= 0)
        {
            content = content.Remove(startIdx, endIdx + BlockMarkerEnd.Length - startIdx);
            await File.WriteAllTextAsync(HostsPath, content.Trim() + Environment.NewLine);
        }
        IsBlocking = false;
    }
}