using FocusVisk.Core.Models;

namespace FocusVisk.Core.Interfaces;

public interface ISettingsRepository
{
    Task<AppSettings?> GetAsync(string userId);
    Task AddAsync(AppSettings settings);
    void Update(AppSettings settings);
    Task<bool> SaveChangesAsync();
}