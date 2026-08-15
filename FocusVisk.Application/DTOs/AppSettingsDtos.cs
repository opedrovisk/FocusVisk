namespace FocusVisk.Application.DTOs;

public class AppSettingsDto
{
    public int PomodoroDurationMinutes { get; set; }
    public int ShortBreakMinutes { get; set; }
    public int LongBreakMinutes { get; set; }
    public int SessionsBeforeLongBreak { get; set; }
    public bool PlaySounds { get; set; }
    public bool ShowNotifications { get; set; }
    public string Theme { get; set; } = "dark";
    public string AccentColor { get; set; } = "#7C6AF7";
    public string SidebarBgColor { get; set; } = "#16161E";
    public string MainBgColor { get; set; } = "#0F0F14";
}

public class AppSettingsUpdateDto : AppSettingsDto { }