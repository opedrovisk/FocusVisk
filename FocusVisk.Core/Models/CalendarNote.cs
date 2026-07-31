namespace FocusVisk.Core.Models;

public class CalendarNote
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;

    public DateTime Date { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? Color { get; set; } = "#7C6AF7";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
