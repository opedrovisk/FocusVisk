namespace FocusVisk.Models;

public enum SearchResultType
{
    Task,
    Note,
    Habit
}

public class SearchResult
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Subtitle { get; set; } = "";
    public string Icon { get; set; } = "";
    public SearchResultType Type { get; set; }
    public string PageTarget { get; set; } = "";
}