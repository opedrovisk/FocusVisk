namespace FocusVisk.Application.DTOs;

public class CalendarNoteDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? Color { get; set; }
}

public class CalendarNoteCreateDto
{
    public DateTime Date { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? Color { get; set; } = "#7C6AF7";
}