using AutoMapper;
using FocusVisk.Application.DTOs;
using FocusVisk.Core.Interfaces;
using FocusVisk.Core.Models;

namespace FocusVisk.Application.Services.Tasks;

public class TaskService : ITaskService
{
    private readonly ITodoRepository _repository;
    private readonly IMapper _mapper;

    public TaskService(ITodoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<TodoItemDto>> GetAllAsync(string userId)
    {
        var tasks = await _repository.GetAllAsync(userId);
        return _mapper.Map<List<TodoItemDto>>(tasks);
    }

    public async Task<TodoItemDto?> GetByIdAsync(int id, string userId)
    {
        var task = await _repository.GetByIdAsync(id, userId);
        return task is null ? null : _mapper.Map<TodoItemDto>(task);
    }

    public async Task<List<TodoItemDto>> GetForMonthAsync(string userId, int year, int month)
    {
        var tasks = await _repository.GetForMonthAsync(userId, year, month);
        return _mapper.Map<List<TodoItemDto>>(tasks);
    }

    public async Task<TodoItemDto> CreateAsync(string userId, TodoItemCreateDto dto)
    {
        var task = _mapper.Map<TodoItem>(dto);
        task.UserId = userId;
        task.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(task);
        await _repository.SaveChangesAsync();

        return _mapper.Map<TodoItemDto>(task);
    }

    public async Task<bool> UpdateAsync(int id, string userId, TodoItemUpdateDto dto)
    {
        var task = await _repository.GetByIdAsync(id, userId);
        if (task is null) return false;

        _mapper.Map(dto, task); 
        _repository.Update(task);
        return await _repository.SaveChangesAsync();
    }

    public async Task<bool> CompleteAsync(int id, string userId)
    {
        var task = await _repository.GetByIdAsync(id, userId);
        if (task is null) return false;

        task.IsCompleted = !task.IsCompleted;
        task.CompletedAt = task.IsCompleted ? DateTime.UtcNow : null;

        _repository.Update(task);
        return await _repository.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var task = await _repository.GetByIdAsync(id, userId);
        if (task is null) return false;

        _repository.Delete(task);
        return await _repository.SaveChangesAsync();
    }
}