using FocusVisk.Core.Enums;

namespace FocusVisk.Core.Models;

public class SearchResult
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Subtitle { get; set; } = "";
    public string Icon { get; set; } = "";
    public SearchResultType Type { get; set; }
    public string PageTarget { get; set; } = "";
}
