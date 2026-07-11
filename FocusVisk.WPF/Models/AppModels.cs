namespace FocusVisk.Models;

public class CalendarNote
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? Color { get; set; } = "#7C6AF7";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class PomodoroSession
{
    public int Id { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int DurationMinutes { get; set; } = 25;
    public bool WasCompleted { get; set; }
    public string? TaskTitle { get; set; }
}

public class QuickNote
{
    public int Id { get; set; }
    public string Title { get; set; } = "Nota sem título";
    public string Content { get; set; } = string.Empty;
    public bool IsPinned { get; set; }
    public string? FolderName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public class AppSettings
{
    public int Id { get; set; }
    public int PomodoroDurationMinutes { get; set; } = 25;
    public int ShortBreakMinutes { get; set; } = 5;
    public int LongBreakMinutes { get; set; } = 15;
    public int SessionsBeforeLongBreak { get; set; } = 4;
    public bool PlaySounds { get; set; } = true;
    public bool ShowNotifications { get; set; } = true;
    public bool FocusBlockEnabled { get; set; } = false;
    public string BlockedSites { get; set; } = "youtube.com\nx.com\ninstagram.com\nreddit.com";

    public string Theme { get; set; } = "dark";
    public string AccentColor { get; set; } = "#7C6AF7";
    public string SidebarBgColor { get; set; } = "#16161E";
    public string MainBgColor { get; set; } = "#0F0F14";

    public string? MidiaResenhaPath { get; set; }
    public bool MidiaResenhaIsVideo { get; set; }
}

public class Habit
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = "fa-solid fa-star";
    public string Color { get; set; } = "#7C6AF7";
    public bool IsArchived { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public List<HabitLog> Logs { get; set; } = new();
}

public class HabitLog
{
    public int Id { get; set; }
    public int HabitId { get; set; }
    public Habit? Habit { get; set; }
    public DateTime Date { get; set; }
}