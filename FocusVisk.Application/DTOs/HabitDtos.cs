namespace FocusVisk.Application.DTOs;

public class HabitDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public bool IsArchived { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CurrentStreak { get; set; }
    public bool CompletedToday { get; set; }
}

public class HabitCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = "fa-solid fa-star";
    public string Color { get; set; } = "#7C6AF7";
}

public class HabitUpdateDto : HabitCreateDto
{
    public bool IsArchived { get; set; }
}