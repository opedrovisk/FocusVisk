using FocusVisk.Application.DTOs;

namespace FocusVisk.Application.Services;

public interface ISearchService
{
    Task<List<SearchResultDto>> SearchAsync(string userId, string query);
}