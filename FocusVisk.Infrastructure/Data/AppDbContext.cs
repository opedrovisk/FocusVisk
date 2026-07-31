using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using FocusVisk.Core.Models;
using FocusVisk.Infrastructure.Identity;

namespace FocusVisk.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TodoItem> Todos => Set<TodoItem>();
    public DbSet<CalendarNote> CalendarNotes => Set<CalendarNote>();
    public DbSet<PomodoroSession> PomodoroSessions => Set<PomodoroSession>();
    public DbSet<QuickNote> QuickNotes => Set<QuickNote>();
    public DbSet<AppSettings> Settings => Set<AppSettings>();
    public DbSet<FinancaTransaction> Transactions => Set<FinancaTransaction>();
    public DbSet<SavingGoal> SavingGoals => Set<SavingGoal>();
    public DbSet<Habit> Habits => Set<Habit>();
    public DbSet<HabitLog> HabitLogs => Set<HabitLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // necessário: registra as tabelas do Identity

        modelBuilder.Entity<FinancaTransaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SavingGoal>()
            .Property(g => g.TargetAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<TodoItem>()
            .Property(t => t.RecurrenceType)
            .HasConversion<int?>();

        modelBuilder.Entity<TodoItem>()
            .HasMany(t => t.SubTasks)
            .WithOne(t => t.Parent)
            .HasForeignKey(t => t.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HabitLog>()
            .HasOne(l => l.Habit)
            .WithMany(h => h.Logs)
            .HasForeignKey(l => l.HabitId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HabitLog>()
            .HasIndex(l => new { l.HabitId, l.Date })
            .IsUnique();

        modelBuilder.Entity<AppSettings>()
            .HasIndex(s => s.UserId)
            .IsUnique();
    }
}

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FocusViskDb;Trusted_Connection=True;")
            .Options;

        return new AppDbContext(options);
    }
}
