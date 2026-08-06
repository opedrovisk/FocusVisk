using AutoMapper;
using FocusVisk.Application.DTOs;
using FocusVisk.Core.Models;

namespace FocusVisk.Application.Mapping;

public class HabitProfile : Profile
{
    public HabitProfile()
    {
        CreateMap<Habit, HabitDto>()
            .ForMember(dest => dest.CurrentStreak, opt => opt.Ignore())  
            .ForMember(dest => dest.CompletedToday, opt => opt.Ignore()); 

        CreateMap<HabitCreateDto, Habit>();
        CreateMap<HabitUpdateDto, Habit>();
    }
}