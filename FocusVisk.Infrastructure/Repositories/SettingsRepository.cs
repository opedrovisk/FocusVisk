using FocusVisk.Core.Interfaces;
using FocusVisk.Core.Models;
using FocusVisk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FocusVisk.Infrastructure.Repositories;

public class SettingsRepository : ISettingsRepository
{
    private readonly AppDbContext _context;
    public SettingsRepository(AppDbContext context) => _context = context;

    public async Task<AppSettings?> GetAsync(string userId) =>
        await _context.Settings.FirstOrDefaultAsync(s => s.UserId == userId);

    public async Task AddAsync(AppSettings settings) => await _context.Settings.AddAsync(settings);
    public void Update(AppSettings settings) => _context.Settings.Update(settings);
    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
}