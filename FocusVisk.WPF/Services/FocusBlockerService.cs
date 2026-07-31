using FocusVisk;
using FocusVisk.Data;
using FocusVisk.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class FocusBlockerService : IDisposable
{
    private const string HostsPath = @"C:\Windows\System32\drivers\etc\hosts";
    private const string BlockMarkerStart = "# === FOCUS FocusVisk START ===";
    private const string BlockMarkerEnd = "# === FOCUS FocusVisk END ===";

    private readonly IServiceProvider _sp;
    private readonly ThemeService _theme;

    private TcpListener? _listener443;
    private TcpListener? _listener80;
    private CancellationTokenSource? _cts;

    private ResenhaPlayerWindow? _playerAtivo;
    private readonly object _playerLock = new();

    public bool IsBlocking { get; private set; }

    public FocusBlockerService(IServiceProvider sp, ThemeService theme)
    {
        _sp = sp;
        _theme = theme;
    }

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
            StartListeners();
        }
        catch (UnauthorizedAccessException)
        {
            throw new InvalidOperationException(
                "Para bloquear sites, execute o FocusVisk como Administrador.");
        }
    }

    public async Task UnblockAsync()
    {
        StopListeners();

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

    private void StartListeners()
    {
        StopListeners();
        _cts = new CancellationTokenSource();

        _listener443 = TryStartTcp(443);
        _listener80 = TryStartTcp(80);

        if (_listener443 != null) _ = AcceptLoopAsync(_listener443, _cts.Token);
        if (_listener80 != null) _ = AcceptLoopAsync(_listener80, _cts.Token);
    }

    private static TcpListener? TryStartTcp(int port)
    {
        try
        {
            var l = new TcpListener(IPAddress.Loopback, port);
            l.Start();
            return l;
        }
        catch { return null; }
    }

    private async Task AcceptLoopAsync(TcpListener listener, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                var client = await listener.AcceptTcpClientAsync(ct);
                client.Close();
                AbrirPlayerSeNecessario();
            }
            catch { break; }
        }
    }

    private void AbrirPlayerSeNecessario()
    {
        if (!_theme.ResenhaAtiva || _theme.MidiaResenhaPath == null) return;

        lock (_playerLock)
        {
            if (_playerAtivo != null && _playerAtivo.IsVisible) return;
        }

        var path = _theme.MidiaResenhaPath;
        var isVideo = _theme.MidiaResenhaIsVideo;

        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            lock (_playerLock)
            {
                if (_playerAtivo != null && _playerAtivo.IsVisible) return;

                var w = new FocusVisk.ResenhaPlayerWindow(path, isVideo);
                w.Closed += (_, _) => { lock (_playerLock) { _playerAtivo = null; } };
                _playerAtivo = w;
                w.Show();
            }
        });
    }

    private void StopListeners()
    {
        _cts?.Cancel();
        try { _listener443?.Stop(); } catch { }
        try { _listener80?.Stop(); } catch { }
        _listener443 = null;
        _listener80 = null;
        _cts = null;
    }

    public void Dispose() => StopListeners();
}