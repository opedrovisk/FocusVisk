namespace FocusVisk.Models;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public string? Tag { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? CompletedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public TimeSpan? ScheduledTime { get; set; }
    public bool IsRecurring { get; set; }
    public RecurrenceType? RecurrenceType { get; set; }
    public DateTime? LastAlertFiredAt { get; set; }
    public bool ShowInCalendar { get; set; }
    public int? ParentId { get; set; }
    public TodoItem? Parent { get; set; }
    public List<TodoItem> SubTasks { get; set; } = new();
}

public enum Priority
{
    Low,
    Medium,
    High
}

public enum RecurrenceType
{
    Daily,
    Weekdays,
    Weekly,
    Monthly
}