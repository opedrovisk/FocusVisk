using AutoMapper;
using FocusVisk.Application.DTOs;
using FocusVisk.Core.Models;

namespace FocusVisk.Application.Mapping;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TodoItem, TodoItemDto>();
        CreateMap<TodoItemCreateDto, TodoItem>();
        CreateMap<TodoItemUpdateDto, TodoItem>();
    }
}