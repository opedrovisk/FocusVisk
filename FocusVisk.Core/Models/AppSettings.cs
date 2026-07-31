namespace FocusVisk.Core.Models;

public class AppSettings
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;

    public int PomodoroDurationMinutes { get; set; } = 25;
    public int ShortBreakMinutes { get; set; } = 5;
    public int LongBreakMinutes { get; set; } = 15;
    public int SessionsBeforeLongBreak { get; set; } = 4;
    public bool PlaySounds { get; set; } = true;
    public bool ShowNotifications { get; set; } = true;

    public string Theme { get; set; } = "dark";
    public string AccentColor { get; set; } = "#7C6AF7";
    public string SidebarBgColor { get; set; } = "#16161E";
    public string MainBgColor { get; set; } = "#0F0F14";
}
