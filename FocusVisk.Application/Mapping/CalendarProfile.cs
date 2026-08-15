using AutoMapper;
using FocusVisk.Application.DTOs;
using FocusVisk.Core.Models;

namespace FocusVisk.Application.Mapping;

public class CalendarProfile : Profile
{
    public CalendarProfile()
    {
        CreateMap<CalendarNote, CalendarNoteDto>();
        CreateMap<CalendarNoteCreateDto, CalendarNote>();
    }
}