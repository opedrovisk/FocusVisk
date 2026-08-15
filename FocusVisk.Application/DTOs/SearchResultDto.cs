using FocusVisk.Core.Enums;

namespace FocusVisk.Application.DTOs;

public class SearchResultDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public SearchResultType Type { get; set; }
    public string PageTarget { get; set; } = string.Empty;
}