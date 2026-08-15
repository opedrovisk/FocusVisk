namespace FocusVisk.Application.DTOs;

public class PomodoroSessionDto
{
    public int Id { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int DurationMinutes { get; set; }
    public bool WasCompleted { get; set; }
    public string? TaskTitle { get; set; }
}

public class PomodoroSessionCreateDto
{
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int DurationMinutes { get; set; }
    public bool WasCompleted { get; set; }
    public string? TaskTitle { get; set; }
}

public class PomodoroStatsDto
{
    public int TotalSessions { get; set; }
    public int CompletedSessions { get; set; }
    public int TotalFocusMinutes { get; set; }
}