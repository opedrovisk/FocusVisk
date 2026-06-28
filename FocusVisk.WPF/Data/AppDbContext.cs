using FocusVisk.Models;
using Microsoft.EntityFrameworkCore;

namespace FocusVisk.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TodoItem> Todos => Set<TodoItem>();
    public DbSet<CalendarNote> CalendarNotes => Set<CalendarNote>();
    public DbSet<PomodoroSession> PomodoroSessions => Set<PomodoroSession>();
    public DbSet<QuickNote> QuickNotes => Set<QuickNote>();
    public DbSet<AppSettings> Settings => Set<AppSettings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed de configurações padrão
        modelBuilder.Entity<AppSettings>().HasData(new AppSettings { Id = 1 });
    }
}
