namespace FocusVisk.Core.Models;

public class Habit
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = "fa-solid fa-star";
    public string Color { get; set; } = "#7C6AF7";
    public bool IsArchived { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastStreakAlertAt { get; set; }
    public List<HabitLog> Logs { get; set; } = new();
}

public class HabitLog
{
    public int Id { get; set; }
    public int HabitId { get; set; }
    public Habit? Habit { get; set; }
    public DateTime Date { get; set; }
}
