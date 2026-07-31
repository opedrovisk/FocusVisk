using FocusVisk.Data;
using FocusVisk.Models;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

public class ThemeService
{
    private readonly IServiceProvider _services;

    public string Theme { get; private set; } = "dark";
    public string AccentColor { get; private set; } = "#7C6AF7";
    public string SidebarBgColor { get; private set; } = "#16161E";
    public string MainBgColor { get; private set; } = "#0F0F14";

    public bool ResenhaAtiva { get; private set; } = false;
    public string? MidiaResenhaPath { get; private set; }
    public bool MidiaResenhaIsVideo { get; private set; }

    public event Action? OnThemeChanged;
    public event Action? OnResenhaChanged;
    public event Action? OnResenhaBloqueiou;

    public ThemeService(IServiceProvider services) => _services = services;

    private AppDbContext Db() =>
        _services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

    public async Task LoadAsync()
    {
        using var db = Db();
        var s = await db.Settings.FindAsync(1);
        if (s == null) return;
        Apply(s.Theme, s.AccentColor, s.SidebarBgColor, s.MainBgColor);

        if (s.MidiaResenhaPath != null && File.Exists(s.MidiaResenhaPath))
        {
            MidiaResenhaPath = s.MidiaResenhaPath;
            MidiaResenhaIsVideo = s.MidiaResenhaIsVideo;
        }
    }

    public async Task SetPresetAsync(string preset)
    {
        var (accent, sidebar, main) = preset switch
        {
            "light" => ("#5A4FD4", "#F0F0F8", "#FAFAFA"),
            _ => ("#7C6AF7", "#16161E", "#0F0F14")
        };
        await SaveAsync(preset, accent, sidebar, main);
    }

    public async Task SetCustomAsync(string accent, string sidebar, string main)
        => await SaveAsync("custom", accent, sidebar, main);

    public void SetResenha(bool ativa)
    {
        ResenhaAtiva = ativa;
        OnResenhaChanged?.Invoke();
    }

    public async Task SetMidiaResenhaAsync(string filePath, bool isVideo)
    {
        MidiaResenhaPath = filePath;
        MidiaResenhaIsVideo = isVideo;

        using var db = Db();
        var s = await db.Settings.FindAsync(1);
        if (s == null) return;
        s.MidiaResenhaPath = filePath;
        s.MidiaResenhaIsVideo = isVideo;
        db.Settings.Update(s);
        await db.SaveChangesAsync();
    }

    public async Task LimparMidiaResenhaAsync()
    {
        if (MidiaResenhaPath != null)
        {
            try { File.Delete(MidiaResenhaPath); } catch { }
        }

        MidiaResenhaPath = null;
        MidiaResenhaIsVideo = false;

        using var db = Db();
        var s = await db.Settings.FindAsync(1);
        if (s == null) return;
        s.MidiaResenhaPath = null;
        s.MidiaResenhaIsVideo = false;
        db.Settings.Update(s);
        await db.SaveChangesAsync();
    }

    public void DispararBloqueio()
    {
        if (ResenhaAtiva && MidiaResenhaPath != null)
            OnResenhaBloqueiou?.Invoke();
    }

    private async Task SaveAsync(string theme, string accent, string sidebar, string main)
    {
        using var db = Db();
        var s = await db.Settings.FindAsync(1) ?? new AppSettings { Id = 1 };
        s.Theme = theme;
        s.AccentColor = accent;
        s.SidebarBgColor = sidebar;
        s.MainBgColor = main;

        if (s.Id == 0) db.Settings.Add(s);
        else db.Settings.Update(s);
        await db.SaveChangesAsync();

        Apply(theme, accent, sidebar, main);
        OnThemeChanged?.Invoke();
    }

    private void Apply(string theme, string accent, string sidebar, string main)
    {
        Theme = theme;
        AccentColor = accent;
        SidebarBgColor = sidebar;
        MainBgColor = main;
    }

    public string BuildCssVariables()
    {
        var accentSecondary = LightenHex(AccentColor, 0.15f);
        var bgSurface = LightenHex(MainBgColor, 0.04f);
        var bgElevated = LightenHex(MainBgColor, 0.08f);
        var bgHover = LightenHex(MainBgColor, 0.12f);
        var bgActive = LightenHex(MainBgColor, 0.18f);

        bool isLight = Theme == "light";
        var textPrimary = isLight ? "#1A1A2E" : "#E8E8F0";
        var textSecondary = isLight ? "#55556A" : "#9090A8";
        var textMuted = isLight ? "#9090A8" : "#5A5A72";
        var border = isLight ? "rgba(0,0,0,0.08)" : "rgba(255,255,255,0.06)";
        var borderStrong = isLight ? "rgba(0,0,0,0.14)" : "rgba(255,255,255,0.12)";

        return $@"
:root {{
  --bg-base:        {MainBgColor};
  --bg-surface:     {bgSurface};
  --bg-elevated:    {bgElevated};
  --bg-hover:       {bgHover};
  --bg-active:      {bgActive};

  --accent-purple:  {AccentColor};
  --accent-purple2: {accentSecondary};

  --sidebar-bg:     {SidebarBgColor};

  --text-primary:   {textPrimary};
  --text-secondary: {textSecondary};
  --text-muted:     {textMuted};
  --text-accent:    {AccentColor};

  --border:         {border};
  --border-strong:  {borderStrong};
}}";
    }

    private static string LightenHex(string hex, float amount)
    {
        try
        {
            hex = hex.TrimStart('#');
            var r = Convert.ToInt32(hex[..2], 16);
            var g = Convert.ToInt32(hex[2..4], 16);
            var b = Convert.ToInt32(hex[4..6], 16);
            r = Math.Min(255, r + (int)(255 * amount));
            g = Math.Min(255, g + (int)(255 * amount));
            b = Math.Min(255, b + (int)(255 * amount));
            return $"#{r:X2}{g:X2}{b:X2}";
        }
        catch { return hex; }
    }
}