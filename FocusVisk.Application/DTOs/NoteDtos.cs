namespace FocusVisk.Application.DTOs;

public class QuickNoteDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsPinned { get; set; }
    public string? FolderName { get; set; }
    public string? Tag { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class QuickNoteCreateDto
{
    public string Title { get; set; } = "Nota sem título";
    public string Content { get; set; } = string.Empty;
    public string? FolderName { get; set; }
    public string? Tag { get; set; }
}

public class QuickNoteUpdateDto : QuickNoteCreateDto
{
    public bool IsPinned { get; set; }
}