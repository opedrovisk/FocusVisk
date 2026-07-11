using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using FocusVisk.Models;

namespace FocusVisk.Data;

public class AppDbContext : DbContext
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
        modelBuilder.Entity<AppSettings>().HasData(new AppSettings { Id = 1 });

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