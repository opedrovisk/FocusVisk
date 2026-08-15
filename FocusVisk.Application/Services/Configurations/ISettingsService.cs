using FocusVisk.Application.DTOs;

namespace FocusVisk.Application.Services;

public interface ISettingsService
{
    Task<AppSettingsDto> GetAsync(string userId);
    Task<AppSettingsDto> UpdateAsync(string userId, AppSettingsUpdateDto dto);
}