namespace FocusVisk.Core.Models;

public class QuickNote
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;

    public string Title { get; set; } = "Nota sem título";
    public string Content { get; set; } = string.Empty;
    public bool IsPinned { get; set; }
    public string? FolderName { get; set; }
    public string? Tag { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
