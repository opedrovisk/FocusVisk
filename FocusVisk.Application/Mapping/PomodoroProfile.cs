using AutoMapper;
using FocusVisk.Application.DTOs;
using FocusVisk.Core.Models;

namespace FocusVisk.Application.Mapping;

public class PomodoroProfile : Profile
{
    public PomodoroProfile()
    {
        CreateMap<PomodoroSession, PomodoroSessionDto>();
        CreateMap<PomodoroSessionCreateDto, PomodoroSession>();
    }
}