using AutoMapper;
using FocusVisk.Application.DTOs;
using FocusVisk.Core.Interfaces;
using CoreModels = FocusVisk.Core.Models;

namespace FocusVisk.Application.Services.Habit;

public class HabitService : IHabitService
{
    private readonly IHabitRepository _repository;
    private readonly IMapper _mapper;

    public HabitService(IHabitRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<HabitDto>> GetAllAsync(string userId)
    {
        var habits = await _repository.GetAllAsync(userId);
        return habits.Select(MapWithStreak).ToList();
    }

    public async Task<HabitDto> CreateAsync(string userId, HabitCreateDto dto)
    {
        var habit = _mapper.Map<CoreModels.Habit>(dto);
        habit.UserId = userId;
        habit.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(habit);
        await _repository.SaveChangesAsync();

        return MapWithStreak(habit);
    }

    public async Task<bool> UpdateAsync(int id, string userId, HabitUpdateDto dto)
    {
        var habit = await _repository.GetByIdAsync(id, userId);
        if (habit is null) return false;

        _mapper.Map(dto, habit);
        _repository.Update(habit);
        return await _repository.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var habit = await _repository.GetByIdAsync(id, userId);
        if (habit is null) return false;

        _repository.Delete(habit);
        return await _repository.SaveChangesAsync();
    }

    public async Task<List<DateTime>> GetLogsForMonthAsync(int habitId, string userId, int year, int month)
    {
        var habit = await _repository.GetByIdAsync(habitId, userId);
        if (habit is null) return new List<DateTime>();

        return await _repository.GetLogsForMonthAsync(habitId, year, month);
    }

    public async Task<bool?> ToggleLogAsync(int habitId, string userId, DateTime date)
    {
        var habit = await _repository.GetByIdAsync(habitId, userId);
        if (habit is null) return null;

        return await _repository.ToggleLogAsync(habitId, date);
    }

    private static HabitDto MapWithStreak(CoreModels.Habit habit)
    {
        return new HabitDto
        {
            Id = habit.Id,
            Name = habit.Name,
            Icon = habit.Icon,
            Color = habit.Color,
            IsArchived = habit.IsArchived,
            CreatedAt = habit.CreatedAt,
            CurrentStreak = ComputeStreak(habit.Logs.Select(l => l.Date).ToList()),
            CompletedToday = habit.Logs.Any(l => l.Date == DateTime.UtcNow.Date)
        };
    }

    private static int ComputeStreak(List<DateTime> logDates)
    {
        var dates = logDates.Select(d => d.Date).ToHashSet();
        var streak = 0;
        var day = DateTime.UtcNow.Date;

        if (!dates.Contains(day)) day = day.AddDays(-1);

        while (dates.Contains(day))
        {
            streak++;
            day = day.AddDays(-1);
        }

        return streak;
    }
}