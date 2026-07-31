namespace FocusVisk.Core.Models;

public class PomodoroSession
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;

    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int DurationMinutes { get; set; } = 25;
    public bool WasCompleted { get; set; }
    public string? TaskTitle { get; set; }
}
