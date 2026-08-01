using FocusVisk.Core.Enums;

namespace FocusVisk.Application.DTOs;

public class TodoItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public Priority Priority { get; set; }
    public string? Tag { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public TimeSpan? ScheduledTime { get; set; }
    public bool IsRecurring { get; set; }
    public RecurrenceType? RecurrenceType { get; set; }
    public bool ShowInCalendar { get; set; }
    public int? ParentId { get; set; }
    public List<TodoItemDto> SubTasks { get; set; } = new();
}

public class TodoItemCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public string? Tag { get; set; }
    public DateTime? DueDate { get; set; }
    public TimeSpan? ScheduledTime { get; set; }
    public bool IsRecurring { get; set; }
    public RecurrenceType? RecurrenceType { get; set; }
    public bool ShowInCalendar { get; set; }
    public int? ParentId { get; set; }
}

public class TodoItemUpdateDto : TodoItemCreateDto
{
    public bool IsCompleted { get; set; }
}